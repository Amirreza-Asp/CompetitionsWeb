using AutoMapper;
using ClosedXML.Excel;
using Competitions.Application.Managment.Interfaces;
using Competitions.Common;
using Competitions.Common.Helpers;
using Competitions.Domain.Dtos.Matches.Matches;
using Competitions.Domain.Entities;
using Competitions.Domain.Entities.Authentication;
using Competitions.Domain.Entities.Managment;
using Competitions.Domain.Entities.Managment.Spec;
using Competitions.Domain.Entities.Places;
using Competitions.Domain.Entities.Static;
using Competitions.Web.Areas.Managment.Models.Matches;
using Competitions.Web.Areas.Matches.Models.Matches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Competitions.Web.Areas.Managment.Controllers
{
    [Area("Matches")]
    [Authorize(Roles = $"{SD.Publisher},{SD.Admin}")]
    public class MatchController : Controller
    {
        private readonly IRepository<Match> _matchRepo;
        private readonly IRepository<MatchConditions> _mcoRepo;
        private readonly IRepository<MatchDocument> _matchDocRepo;
        private readonly IRepository<Place> _placeRepo;
        private readonly IRepository<Festival> _festivalRepo;
        private readonly IRepository<AudienceType> _audRepo;
        private readonly IMapper _mapper;
        private readonly IRepository<Evidence> _evdRepo;
        private readonly IMatchService _matchService;
        private readonly IRepository<Team> _teamRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<UserTeam> _userTeamRepo;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostEnvironment;

        private static MatchFilter _filters = new MatchFilter();

        public MatchController(IRepository<Match> matchRepo, IMapper mapper,
            IRepository<Festival> festivalRepo, IRepository<Place> placeRepo,
            IRepository<AudienceType> audRepo, IRepository<MatchConditions> mcoRepo,
            IRepository<MatchDocument> matchDocRepo, IRepository<Evidence> evdRepo, IMatchService matchService,
            IRepository<Team> teamRepo, IRepository<User> userRepo, IRepository<UserTeam> userTeamRepo, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostEnvironment)
        {
            _matchRepo = matchRepo;
            _mapper = mapper;
            _festivalRepo = festivalRepo;
            _placeRepo = placeRepo;
            _audRepo = audRepo;
            _mcoRepo = mcoRepo;
            _matchDocRepo = matchDocRepo;
            _evdRepo = evdRepo;
            _matchService = matchService;
            _teamRepo = teamRepo;
            _userRepo = userRepo;
            _userTeamRepo = userTeamRepo;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index(MatchFilter filters)
        {
            _filters = filters;

            var spec = new GetFilteredMatchSpec(filters.Skip, filters.Take);
            filters.Total = _matchRepo.GetCount(spec);

            var vm = new GetAllMatchesVM
            {
                Matches = _matchRepo.GetAll(spec, select: entity => _mapper.Map<MatchDetailsDto>(entity)),
                Filters = filters
            };

            return View(vm);
        }

        #region Create/Update/Details

        // step 1
        private static MatchFirstInfoDto? step1;
        public async Task<IActionResult> FirstInfo(Guid? id, bool readOnly)
        {
            if (step1 != null && step1.Id == id && !readOnly)
            {
                step1 = await FillLists(step1);
                return View(step1);
            }

            var command = new MatchFirstInfoDto() { ReadOnly = readOnly };

            // Update
            if (id.HasValue)
            {
                var entity = await _matchRepo.FindAsync(id);
                if (entity == null)
                {
                    TempData[SD.Error] = "مسابقه انتخاب شده وجود ندارد";
                    return RedirectToAction(nameof(Index), _filters);
                }

                command = _mapper.Map<MatchFirstInfoDto>(entity);
                command.ReadOnly = readOnly;
                var audiences = await _matchRepo.FirstOrDefaultSelectAsync(
                    filter: u => u.Id == id,
                    include: source => source.Include(u => u.AudienceTypes),
                    select: u => u.AudienceTypes);

                command.Audience = String.Join(',', audiences.Select(u => u.AudienceTypeId));

            }

            command = await FillLists(command);
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> FirstInfo(MatchFirstInfoDto command)
        {
            command.StartPutOn = command.StartPutOn.ToMiladi();
            command.EndPutOn = command.EndPutOn.ToMiladi();
            command.StartRegister = command.StartRegister.ToMiladi();
            command.EndRegister = command.EndRegister.ToMiladi();

            if (!ModelState.IsValid)
            {
                command = await FillLists(command);
                return View(command);
            }

            if (command.StartRegister.ToDateTime() > command.EndRegister.ToDateTime())
            {
                TempData[SD.Error] = "تاریخ شروع ثبت نام نمیتواند از پایان ان بیشتر باشد";

                command = await FillLists(command);
                return View(command);
            }

            if (command.EndRegister.ToDateTime() > command.StartPutOn.ToDateTime())
            {
                TempData[SD.Error] = "تاریخ پایان ثبت نام نمیتواند از شروع مسابقات بیشتر باشد";

                command = await FillLists(command);
                return View(command);
            }


            if (command.StartPutOn.ToDateTime() > command.EndPutOn.ToDateTime())
            {
                TempData[SD.Error] = "تاریخ برگزاری مسابقه نمیتواند از اتمام ان بیشتر باشد";

                command = await FillLists(command);
                return View(command);
            }

            step1 = command;

            if (!command.Id.HasValue)
                return RedirectToAction(nameof(SecondInfo));
            else
                return RedirectToAction(nameof(SecondInfo), new { id = command.Id.Value });

        }

        private async Task<MatchFirstInfoDto> FillLists(MatchFirstInfoDto command)
        {
            command.Places = await _placeRepo.GetAllAsync(
                 filter: u => u.ParentPlaceId == null,
                 select: entity => new SelectListItem { Text = entity.Title, Value = entity.Id.ToString() });

            command.Festivals = await _festivalRepo.GetAllAsync(
                    filter: u => u.Duration.From <= DateTime.Now && u.Duration.To >= DateTime.Now,
                    select: entity => new SelectListItem { Text = entity.Title, Value = entity.Id.ToString() });

            command.AudienceTypes = await _audRepo.GetAllAsync(select: entity => new SelectListItem { Text = entity.Title, Value = entity.Id.ToString() });

            return command;
        }

        // step 2
        private static MatchSecondInfoDto? step2;
        public async Task<IActionResult> SecondInfo(Guid? id, bool readOnly)
        {
            if (step2 != null && step2.Id == id && !readOnly)
                return View(step2);

            var command = new MatchSecondInfoDto() { ReadOnly = readOnly };
            // Update
            if (id.HasValue)
            {
                var entity = await _matchRepo.FindAsync(id);
                command.Id = entity.Id;
                command.Image = entity.Image.Name;
                command.Description = entity.Description;
            }

            return View(command);
        }
        [HttpPost]
        public IActionResult SecondInfo(MatchSecondInfoDto command)
        {
            if (!ModelState.IsValid)
                return View(command);


            var files = HttpContext.Request.Form.Files;
            if (!files.Any() && !command.Id.HasValue)
            {
                TempData[SD.Error] = "تصویر مسابقه را وارد کنید";
                return View(command);
            }

            if (files.Any() && files[0].ReadBytes().Length > SD.ImageSizeLimit)
            {
                TempData[SD.Error] = $"سایز عکس وارد شده باید کمتر از {SD.ImageSizeLimitDisplay} باشد";
                return View(command);
            }

            if (files.Any())
            {
                command.ImageFile = files[0].ReadBytes();
                command.Image = files[0].FileName;
            }
            step2 = command;


            if (!command.Id.HasValue)
                return RedirectToAction(nameof(MatchCondition));
            else
                return RedirectToAction(nameof(MatchCondition), new { id = command.Id.Value });
        }


        // step 3
        private static MatchConditionDto? step3;
        public async Task<IActionResult> MatchCondition(Guid? id, bool readOnly)
        {
            if (step3 != null && step3.Id == id && !readOnly)
                return View(step3);

            var command = new MatchConditionDto();
            // update or details
            if (id.HasValue)
            {
                var entity = await _mcoRepo.FirstOrDefaultAsync(u => u.MatchId == id);
                if (entity == null)
                {
                    TempData[SD.Error] = "مسابقه وارد شده وجود ندارد";
                    return RedirectToAction(nameof(Index), _filters);
                }

                command.Id = entity.MatchId;
                command.Payment = entity.Payment;
                command.Free = entity.Free;
                command.File = entity.Regulations.Name;
            }
            command.ReadOnly = readOnly;
            return View(command);
        }
        [HttpPost]
        public IActionResult MatchCondition(MatchConditionDto command)
        {
            if (!ModelState.IsValid)
                return View(command);

            var files = HttpContext.Request.Form.Files;
            if (!files.Any() && !command.Id.HasValue)
            {
                TempData[SD.Error] = "آیین نامه مسابقات را وارد کنید";
                return View(command);
            }

            if (files.Any())
            {
                command.RGFile = files[0].ReadBytes();
                command.File = files[0].FileName;
            }
            step3 = command;


            if (!command.Id.HasValue)
                return RedirectToAction(nameof(MatchDocument));
            else
                return RedirectToAction(nameof(MatchDocument), new { id = command.Id.Value });
        }


        // step 4
        private static MatchDocumentDto? step4;
        public async Task<IActionResult> MatchDocument(Guid? id, bool readOnly)
        {
            if (step4 != null && step4.Id == id && !readOnly)
            {
                step4.Evidences = await _evdRepo.GetAllAsync(select: u => new SelectListItem(u.Title, u.Id.ToString()));
                return View(step4);
            }

            var docs = new MatchDocumentDto() { ReadOnly = readOnly };
            // update
            if (id.HasValue)
            {
                docs = await _matchRepo.FirstOrDefaultSelectAsync(
                   filter: u => u.Id == id,
                   select: u => new MatchDocumentDto
                   {
                       Id = id,
                       Data = "[" + String.Join(',', u.Documents.Select(u => u.ToJson())) + "]",
                       Info = u.Documents.Select(b => new DocumentDataDto { EvidenceId = b.EvidenceId, Type = b.Type })
                   });
            }

            docs.Evidences = await _evdRepo.GetAllAsync(select: u => new SelectListItem(u.Title, u.Id.ToString()));
            docs.ReadOnly = readOnly;
            return View(docs);
        }
        [HttpPost]
        public async Task<IActionResult> MatchDocument(MatchDocumentDto command)
        {
            command.Info = JsonConvert.DeserializeObject<List<DocumentDataDto>>(command.Data);


            if (!ModelState.IsValid)
            {
                command.Evidences = await _evdRepo.GetAllAsync(select: u => new SelectListItem(u.Title, u.Id.ToString()));
                return View(command);
            }

            step4 = command;
            if (!command.Id.HasValue)
                return RedirectToAction(nameof(MatchAward));
            else
                return RedirectToAction(nameof(MatchAward), new { id = command.Id.Value });
        }


        // step 5
        private static MatchAwardListDto? step5;
        public async Task<IActionResult> MatchAward(Guid? id, bool readOnly)
        {
            if (step5 != null && step5.Id == id && !readOnly)
                return View(step5);

            var dto = new MatchAwardListDto() { Id = id, ReadOnly = readOnly };
            // update
            if (id.HasValue)
            {
                dto.Info = await _matchRepo.FirstOrDefaultSelectAsync(
                        filter: u => u.Id == id.Value,
                        include: source => source.Include(u => u.Awards),
                        select: entity => entity.Awards.Select(u => new MatchAwardDto { Rank = u.Score, Prize = u.Prize }));
            }

            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> MatchAward(MatchAwardListDto command)
        {
            command.Info = JsonConvert.DeserializeObject<List<MatchAwardDto>>(command.Data);
            step5 = command;

            if (!command.Id.HasValue)
            {
                await _matchService.CreateAsync(step1, step2, step3, step4, step5);
                TempData[SD.Success] = "مسابقه با موفقیت ذخیره شد";
            }
            else
            {
                await _matchService.UpdateAsync(step1, step2, step3, step4, step5);
                TempData[SD.Info] = "ویرایش مسابقه با موفقیت انجام شد";
            }
            step1 = null;
            step2 = null;
            step3 = null;
            step4 = null;
            step5 = null;

            return RedirectToAction(nameof(Index), _filters);
        }
        #endregion

        public async Task<IActionResult> ShowTeams(Guid id)
        {
            var teams = await _teamRepo.GetAllAsync(
                filter: u => u.MatchId == id && u.Users.Count() > 0,
                include: source => source
                .Include(u => u.Users)
                .ThenInclude(u => u.User));

            return View(teams);
        }

        public async Task<IActionResult> TeamInfo(Guid teamId)
        {
            var data = await _teamRepo.FirstOrDefaultAsync(
                    u => u.Id == teamId,
                    include: source => source.Include(u => u.Users)
                        .ThenInclude(u => u.User));

            return View(data);
        }


        public async Task<IActionResult> UserInfo(Guid id)
        {
            var user = await _userRepo.FindAsync(id);
            if (user == null)
            {
                TempData[SD.Error] = "کاربر انتخاب شده وجود ندارد";
                return Redirect(Request.GetTypedHeaders().Referer.ToString());
            }

            var userInfoVM = new UserInfoVM
            {
                User = user,
                UserTeam = await _userTeamRepo.FirstOrDefaultAsync(u => u.UserId == user.Id,
                include: source => source.Include(u => u.Documents))
            };

            return View(userInfoVM);
        }

        public async Task<FileResult> DownloadDoc(long id)
        {
            var userTeam = await _userTeamRepo
                .FirstOrDefaultAsync(
                    u => u.Documents.Any(u => u.Id == id),
                    include: source => source.Include(u => u.Documents));

            var doc = userTeam.Documents.First(u => u.Id == id);

            if (doc == null)
                throw new Exception("document not found");

            var extension = Path.GetExtension(doc.File.Name);
            string filePath = _hostEnvironment.WebRootPath + StaticEntitiesDetails.UserTeamDocPath + doc.File.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", doc.Name + extension);
        }

        public async Task<FileResult> DownloadCondition(String fileName)
        {
            var condition = await _mcoRepo
                .FirstOrDefaultAsync(
                    u => u.Regulations.Name == fileName);

            if (condition == null)
                throw new Exception("document not found");

            var extension = Path.GetExtension(condition.Regulations.Name);
            string filePath = _hostEnvironment.WebRootPath + StaticEntitiesDetails.MatchConditionsRegulationsPath + condition.Regulations.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", "آیین نامه" + extension);
        }

        [HttpDelete]
        public async Task<JsonResult> Remove(Guid id)
        {
            await _matchService.RemoveAsync(id);
            return Json(new { Success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> RemoveTeam(Guid id)
        {
            var team = await _teamRepo.FirstOrDefaultAsync(b => b.Id == id);

            if (team == null)
                return Json(new { Success = true });

            _teamRepo.Remove(team);
            await _teamRepo.SaveAsync();

            return Json(new { Success = true });
        }

        public async Task<IActionResult> PrintExcel(Guid matchId)
        {
            var match =
                await _matchRepo.FirstOrDefaultAsync(
                    m => m.Id == matchId,
                    include: source => source.Include(u => u.Teams).ThenInclude(b => b.Users).ThenInclude(b => b.User));

            if (match == null)
                return null;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Match_Players");

                var currentRow = 1;

                #region header
                worksheet.Cell(currentRow, 1).Value = "نام";
                worksheet.Cell(currentRow, 2).Value = "نام خانوادگی";
                worksheet.Cell(currentRow, 3).Value = "شماره تلفن";
                #endregion

                int teamCount = 0;
                #region body
                foreach (var team in match.Teams)
                {
                    teamCount++;
                    foreach (var userTeam in team.Users)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = userTeam.User.Name;
                        worksheet.Cell(currentRow, 2).Value = userTeam.User.Family;
                        worksheet.Cell(currentRow, 3).Value = String.IsNullOrEmpty(userTeam.User.PhoneNumber) ? "در سیستم وچود ندارد" : userTeam.User.PhoneNumber.Value;
                    }
                }
                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Students.xlsx"
                        );
                }
            }
        }
    }
}
