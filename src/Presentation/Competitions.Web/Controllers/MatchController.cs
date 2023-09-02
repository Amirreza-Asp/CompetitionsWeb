using Competitions.Common;
using Competitions.Common.Helpers;
using Competitions.Domain.Dtos.Matches.Matches;
using Competitions.Domain.Entities;
using Competitions.Domain.Entities.Authentication;
using Competitions.Domain.Entities.Managment;
using Competitions.SharedKernel.ValueObjects;
using Competitions.Web.Models.Calenders;
using Competitions.Web.Models.Matches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Competitions.Web.Controllers
{
    [Authorize]
    public class MatchController : Controller
    {
        private readonly IRepository<Match> _matchRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Team> _teamRepo;
        private readonly IRepository<UserTeam> _userTeamRepo;
        private readonly IRepository<MatchDocument> _docRepo;
        private readonly IRepository<UserTeamDocument> _userTeamDocRepo;
        private readonly IRepository<MatchConditions> _mcoRepo;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        private static MatchFilterDto _filters = new MatchFilterDto();
        private static MatchCalenderFilter _calenderFilters = new MatchCalenderFilter();

        public MatchController(IRepository<Match> matchRepo, IRepository<User> userRepo, IRepository<UserTeam> userTeamRepo, IRepository<Team> teamRepo, IRepository<MatchDocument> docRepo, IRepository<UserTeamDocument> userTeamDocRepo, IRepository<MatchConditions> mcoRepo, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _matchRepo = matchRepo;
            _userRepo = userRepo;
            _userTeamRepo = userTeamRepo;
            _teamRepo = teamRepo;
            _docRepo = docRepo;
            _userTeamDocRepo = userTeamDocRepo;
            _mcoRepo = mcoRepo;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index(MatchFilterDto filters)
        {
            _filters = filters;

            filters.Total = _matchRepo.GetCount(u => (
                !filters.MatchDate.HasValue || u.PutOn.From.Date.Equals(filters.MatchDate.Value.Date)) &&
                (String.IsNullOrEmpty(filters.Level) || u.Level.Equals(filters.Level)));

            var vm = new MatchListVM
            {
                Matches = await _matchRepo.GetAllAsync(
                   filter: u =>
                       (u.Gender.ToString() == User.FindFirstValue(ClaimTypes.Gender) ||
                        User.IsInRole(SD.Admin) || User.IsInRole(SD.Publisher)) &&
                       (!filters.MatchDate.HasValue || u.PutOn.From.Date.Equals(filters.MatchDate.Value.Date)) &&
                       (String.IsNullOrEmpty(filters.Level) || u.Level.Equals(filters.Level)),
                    orderBy: source => source.OrderByDescending(u => u.CreateDate),
                    include: source => source
                            .Include(u => u.Sport)
                                .ThenInclude(u => u.SportType)
                            .Include(u => u.Place),
                    take: filters.Take,
                    skip: filters.Skip),
                Filters = filters
            };

            return View(vm);
        }


        public IActionResult Calender(MatchCalenderFilter filters)
        {
            filters.MatchDate = _calenderFilters.MatchDate;

            if (String.IsNullOrEmpty(filters.Level))
                filters.Level = _calenderFilters.Level;

            filters.Move();
            filters.RoundMeetingsDate();
            _calenderFilters = filters;

            return View(filters);
        }

        public async Task<JsonResult> CalenderInfo()
        {
            var LastDateOfTheMonth = _calenderFilters.MatchDate.GetTheLastDateOfTheMonth();
            var matches = await _matchRepo.GetAllAsync(
             filter: u =>
             (u.Gender.ToString() == User.FindFirstValue(ClaimTypes.Gender) ||
                        User.IsInRole(SD.Admin) || User.IsInRole(SD.Publisher)) &&
             u.Level.Equals(_calenderFilters.Level) &&
             (u.PutOn.To.Date >= _calenderFilters.MatchDate.Date ||
             u.PutOn.From.Date >= _calenderFilters.MatchDate.Date) &&
             u.PutOn.From.Date <= LastDateOfTheMonth.Date,
             select: u => new CalenderMatchData(u.Name, u.PutOn.From, u.PutOn.To, $"/Match/Details/{u.Id}", u.Gender));

            return Json(new { Success = true, Data = matches });
        }

        public async Task<IActionResult> Register(Guid id)
        {
            var match = await _matchRepo.FirstOrDefaultAsync(u => u.Id == id,
                    include: source => source.Include(u => u.Conditions)
                            .Include(u => u.Documents)
                             .ThenInclude(u => u.Evidence));

            if (match == null)
            {
                TempData[SD.Error] = "مسابقه انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index), _filters);
            }

            var matchList = new RegisterMatchListDto()
            {
                MatchId = match.Id,
                TeamCount = match.TeamCount,
                Documents = match.Documents,
                Gender = match.Gender,
                RegisterMatches = new List<RegisterMatchDto>()
            };

            for (int i = 0; i < match.TeamCount; i++)
                matchList.RegisterMatches.Add(new RegisterMatchDto());

            return View(matchList);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterMatchListDto command)
        {
            var match = await _matchRepo.FirstOrDefaultAsync(u => u.Id == command.MatchId,
                include: source => source.Include(u => u.Conditions)
                            .Include(u => u.Documents)
                                .ThenInclude(u => u.Evidence));

            command.Documents = match.Documents;

            if (!ModelState.IsValid)
                return View(command);

            if (match == null)
            {
                TempData[SD.Error] = "مسابقه انتخاب شده وجود نداد";
                return RedirectToAction(nameof(Index));
            }

            var files = HttpContext.Request.Form.Files;

            if (files.Count() != command.Documents.Count() * command.TeamCount)
            {
                TempData[SD.Error] = "لطفا اطلاعات را کامل کنید";
                return View(command);
            }

            for (int i = 0; i < command.RegisterMatches.Count; i++)
            {
                var actor = User.FindFirstValue(ClaimTypes.Actor);

                var user = await _userRepo.FirstOrDefaultAsync(u =>
                        u.NationalCode.Value == command.RegisterMatches[i].NationalCode.ToString());

                var userFiles = HttpContext.Request.Form.Files
                    .Skip(i * command.Documents.Count()).Take(command.Documents.Count());

                if (InvalidRegister(user, match.Id, command.RegisterMatches[i], userFiles, command.Gender, command.Documents.Count()))
                {
                    return View(command);
                }

                foreach (var file in userFiles)
                    command.RegisterMatches[i].FilesBytes.Add(new(file.FileName, file.ReadBytes()));
            }

            Team team = new Team(match.Id);
            int count = 0;
            foreach (var item in command.RegisterMatches)
            {
                var userId = _userRepo.FirstOrDefaultSelect(filter: u => u.NationalCode.Value == item.NationalCode.ToString(), select: u => u.Id);
                var userTeam = new UserTeam(team.Id, userId, count == 0);
                count++;

                for (int i = 0; item.FilesBytes != null && i < item.FilesBytes.Count(); i++)
                {
                    var doc = new UserTeamDocument(item.FilesNames[i], new Document(item.FilesBytes[i].Name, item.FilesBytes[i].Bytes), userTeam.Id);
                    doc.SaveFile();
                    userTeam.AddDoc(doc);
                }

                team.AddUser(userTeam);
            }

            _teamRepo.Add(team);
            await _teamRepo.SaveAsync();

            TempData[SD.Success] = command.TeamCount == 1 ? "با موفقیت وارد مسابقه شدید" : "تیم وارد شده با موفقیت به مسابقه اضافه شدند";
            return RedirectToAction(nameof(Index), _filters);
        }


        private bool InvalidRegister(User user, Guid matchId, RegisterMatchDto command, IEnumerable<IFormFile> files, bool gender, int docCount)
        {

            if (user == null)
            {
                TempData[SD.Warning] = $"کد ملی {command.NationalCode} در سیستم ثبت نشده";
                return true;
            }

            if (user.Gender != gender)
            {
                TempData[SD.Error] = "جنسیت شخص انتخاب شده خلاف قوانین مسابقه است";
                return true;
            }

            if (_userTeamRepo.FirstOrDefault(u => u.UserId == user.Id && u.Team.Match.Id == matchId) != null)
            {
                TempData[SD.Warning] = $"کاربر {user.Name + ' ' + user.Family} با شماره دانشجویی {user.StudentNumber.Value} قبلا در تیمی دیگر عضو شده است";
                return true;
            }

            if (files.Count() < docCount)
            {
                TempData[SD.Error] = "فایل های لازم را کامل کنید";
                return true;
            }

            return false;

        }

        public async Task<IActionResult> Details(Guid id)
        {
            var match = await _matchRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id,
                include: source => source
                    .Include(u => u.Festival)
                    .Include(u => u.Place)
                    .Include(u => u.Sport)
                    .Include(u => u.AudienceTypes)
                        .ThenInclude(u => u.AudienceType)
                    .Include(u => u.Awards)
                    .Include(u => u.Conditions)
                    .Include(u => u.Documents)
                        .ThenInclude(u => u.Evidence));

            if (match == null)
            {
                TempData[SD.Error] = "مسابقه انتخاب شده در سیستم وجود ندارد";
                return Redirect(Request.GetTypedHeaders().Referer.ToString());
            }

            return View(match);
        }

        public async Task<FileResult> DownloadCondition(String fileName)
        {
            var condition = await _mcoRepo
                .FirstOrDefaultAsync(
                    u => u.Regulations.Name == fileName);

            if (condition == null)
                throw new Exception("document not found");

            var extension = Path.GetExtension(condition.Regulations.Name);
            string filePath = _hostingEnvironment.WebRootPath + StaticEntitiesDetails.MatchConditionsRegulationsPath + condition.Regulations.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", "آیین نامه" + extension);
        }
    }
}
