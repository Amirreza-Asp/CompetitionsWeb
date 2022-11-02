using Competitions.Common;
using Competitions.Common.Helpers;
using Competitions.Domain.Dtos.Matches.Matches;
using Competitions.Domain.Entities.Authentication;
using Competitions.Domain.Entities.Managment;
using Competitions.SharedKernel.ValueObjects;
using Competitions.Web.Models.Calenders;
using Competitions.Web.Models.Matches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        private static MatchFilterDto _filters = new MatchFilterDto();
        private static MatchCalenderFilter _calenderFilters = new MatchCalenderFilter();

        public MatchController ( IRepository<Match> matchRepo , IRepository<User> userRepo , IRepository<UserTeam> userTeamRepo , IRepository<Team> teamRepo , IRepository<MatchDocument> docRepo , IRepository<UserTeamDocument> userTeamDocRepo )
        {
            _matchRepo = matchRepo;
            _userRepo = userRepo;
            _userTeamRepo = userTeamRepo;
            _teamRepo = teamRepo;
            _docRepo = docRepo;
            _userTeamDocRepo = userTeamDocRepo;
        }

        public async Task<IActionResult> Index ( MatchFilterDto filters )
        {
            _filters = filters;

            filters.Total = _matchRepo.GetCount(u => (
                !filters.MatchDate.HasValue || u.PutOn.From.Date.Equals(filters.MatchDate.Value.Date) ) &&
                ( String.IsNullOrEmpty(filters.Level) || u.Level.Equals(filters.Level) ));

            var vm = new MatchListVM
            {
                Matches = await _matchRepo.GetAllAsync(
                   filter: u => ( !filters.MatchDate.HasValue || u.PutOn.From.Date.Equals(filters.MatchDate.Value.Date) ) &&
                                  ( String.IsNullOrEmpty(filters.Level) || u.Level.Equals(filters.Level) ) ,
                    orderBy: source => source.OrderByDescending(u => u.CreateDate) ,
                    include: source => source
                            .Include(u => u.Sport)
                                .ThenInclude(u => u.SportType)
                            .Include(u => u.Place) ,
                    take: filters.Take ,
                    skip: filters.Skip) ,
                Filters = filters
            };

            return View(vm);
        }


        public IActionResult Calender ( MatchCalenderFilter filters )
        {
            filters.MatchDate = _calenderFilters.MatchDate;

            if ( String.IsNullOrEmpty(filters.Level) )
                filters.Level = _calenderFilters.Level;

            filters.Move();
            filters.RoundMeetingsDate();
            _calenderFilters = filters;

            return View(filters);
        }

        public async Task<JsonResult> CalenderInfo ()
        {
            var LastDateOfTheMonth = _calenderFilters.MatchDate.GetTheLastDateOfTheMonth();
            var matches = await _matchRepo.GetAllAsync(
             filter: u =>
             u.Level.Equals(_calenderFilters.Level) &&
             ( u.PutOn.To.Date >= _calenderFilters.MatchDate.Date ||
             u.PutOn.From.Date >= _calenderFilters.MatchDate.Date ) &&
             u.PutOn.From.Date <= LastDateOfTheMonth.Date ,
             select: u => new CalenderMatchData(u.Name , u.PutOn.From , u.PutOn.To , $"/Match/Details/{u.Id}" , u.Gender));

            return Json(new { Success = true , Data = matches });
        }


        private static RegisterMatchListDto matchList = new RegisterMatchListDto();
        public async Task<IActionResult> Register ( Guid id )
        {
            var match = await _matchRepo.FirstOrDefaultAsync(u => u.Id == id ,
                include: source => source.Include(u => u.Conditions)
                        .Include(u => u.Documents)
                         .ThenInclude(u => u.Evidence));

            if ( match == null )
            {
                TempData[SD.Error] = "مسابقه انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            matchList = new RegisterMatchListDto()
            {
                MatchId = match.Id ,
                TeamCount = match.TeamCount ,
                Documents = match.Documents ,
                Gender = match.Gender ,
                RegisterMatches = new List<RegisterMatchDto>()
            };

            var rg = new RegisterMatchDto() { Number = 1 , TeamCount = match.TeamCount , Documents = match.Documents };

            return View(rg);
        }
        [HttpPost]
        public async Task<IActionResult> Register ( RegisterMatchDto command )
        {
            command.Documents = matchList.Documents;
            if ( !ModelState.IsValid )
                return View(command);

            var user = await _userRepo.FirstOrDefaultAsync(u => u.StudentNumber.Value == command.StudentNumber.ToString());
            if ( user == null )
            {
                TempData[SD.Warning] = $"شماره دانشجویی {command.StudentNumber} در سیستم ثبت نشده";
                return View(command);
            }

            if ( user.Gender != matchList.Gender )
            {
                TempData[SD.Error] = "جنسیت شخص انتخاب شده خلاف قوانین مسابقه است";
                return View(command);
            }

            if ( _userTeamRepo.FirstOrDefault(u => u.UserId == user.Id && u.Team.Match.Id == matchList.MatchId) != null )
            {
                TempData[SD.Warning] = $"کاربر {user.Name + ' ' + user.Family} با شماره دانشجویی {user.StudentNumber.Value} قبلا در تیمی دیگر عضو شده است";
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            if ( files.Count() < matchList.Documents.Count() )
            {
                TempData[SD.Error] = "فایل های لازم را کامل کنید";
                return View(command);
            }


            command.FilesBytes = new List<(string Name, byte[] Bytes)>();
            foreach ( var file in files )
                command.FilesBytes.Add(new(file.FileName , file.ReadBytes()));

            matchList.RegisterMatches.Add(command);

            if ( matchList.TeamCount != matchList.RegisterMatches.Count() )
            {
                var rg = new RegisterMatchDto()
                {
                    TeamCount = matchList.TeamCount ,
                    Number = matchList.RegisterMatches.Count() + 1 ,
                    Documents = matchList.Documents
                };

                return View(rg);
            }

            Team team = new Team(matchList.MatchId);
            foreach ( var item in matchList.RegisterMatches )
            {
                var userId = _userRepo.FirstOrDefaultSelect(filter: u => u.StudentNumber.Value == item.StudentNumber.ToString() , select: u => u.Id);
                var userTeam = new UserTeam(team.Id , userId);

                for ( int i = 0 ; item.FilesBytes != null && i < item.FilesBytes.Count() ; i++ )
                {
                    var doc = new UserTeamDocument(item.FilesNames[i] , new Document(item.FilesBytes[i].Name , item.FilesBytes[i].Bytes) , userTeam.Id);
                    doc.SaveFile();
                    userTeam.AddDoc(doc);
                }

                team.AddUser(userTeam);
            }

            _teamRepo.Add(team);
            await _teamRepo.SaveAsync();

            TempData[SD.Success] = matchList.TeamCount == 1 ? "شخص انتخاب شده با موفقیت وارد مسابقه شد" : "تیم وارد شده با موفقیت به مسابقه اضافه شدند";
            return RedirectToAction(nameof(Index) , _filters);
        }


        public async Task<IActionResult> Details ( Guid id )
        {
            var match = await _matchRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
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

            if ( match == null )
            {
                TempData[SD.Error] = "مسابقه انتخاب شده در سیستم وجود ندارد";
                return Redirect(Request.GetTypedHeaders().Referer.ToString());
            }

            return View(match);
        }
    }
}
