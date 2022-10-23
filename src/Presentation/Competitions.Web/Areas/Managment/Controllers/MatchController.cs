using AutoMapper;
using Competitions.Application.Managment.Interfaces;
using Competitions.Common;
using Competitions.Common.Helpers;
using Competitions.Domain.Dtos.Managment.Matches;
using Competitions.Domain.Entities.Managment;
using Competitions.Domain.Entities.Managment.Spec;
using Competitions.Domain.Entities.Places;
using Competitions.Domain.Entities.Static;
using Competitions.Web.Areas.Managment.Models.Matches;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Competitions.Web.Areas.Managment.Controllers
{
    [Area("Managment")]
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

        private static MatchFilter _filters = new MatchFilter();

        public MatchController ( IRepository<Match> matchRepo , IMapper mapper , IRepository<Festival> festivalRepo , IRepository<Place> placeRepo , IRepository<AudienceType> audRepo , IRepository<MatchConditions> mcoRepo , IRepository<MatchDocument> matchDocRepo , IRepository<Evidence> evdRepo , IMatchService matchService )
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
        }

        public IActionResult Index ( MatchFilter filters )
        {
            _filters = filters;

            var spec = new GetFilteredMatchSpec(filters.Skip , filters.Take);
            filters.Total = _matchRepo.GetCount(spec);

            var vm = new GetAllMatchesVM
            {
                Matches = _matchRepo.GetAll(spec , select: entity => _mapper.Map<MatchDetailsDto>(entity)) ,
                Filters = filters
            };

            return View(vm);
        }


        #region Create/Update/Details

        // step 1
        private static MatchFirstInfoDto? step1;
        public async Task<IActionResult> FirstInfo ( Guid? id , bool readOnly )
        {
            if ( step1 != null && step1.Id == id && !readOnly )
            {
                step1 = await FillLists(step1);
                return View(step1);
            }

            var command = new MatchFirstInfoDto() { ReadOnly = readOnly };

            // Update
            if ( id.HasValue )
            {
                var entity = await _matchRepo.FindAsync(id);
                if ( entity == null )
                {
                    TempData[SD.Error] = "مسابقه انتخاب شده وجود ندارد";
                    return RedirectToAction(nameof(Index) , _filters);
                }

                command = _mapper.Map<MatchFirstInfoDto>(entity);
                command.ReadOnly = readOnly;
                var audiences = await _matchRepo.FirstOrDefaultSelectAsync(
                    filter: u => u.Id == id ,
                    include: source => source.Include(u => u.AudienceTypes) ,
                    select: u => u.AudienceTypes);

                command.Audience = String.Join(',' , audiences.Select(u => u.AudienceTypeId));

            }

            command = await FillLists(command);
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> FirstInfo ( MatchFirstInfoDto command )
        {
            if ( !ModelState.IsValid )
            {
                command = await FillLists(command);
                command = ChangePutOn(command);
                return View(command);
            }

            if ( DateTimeConvertor.GetDateFromString(command.StartRegister) > DateTimeConvertor.GetDateFromString(command.EndRegister) )
            {
                TempData[SD.Error] = "تاریخ شروع ثبت نام نمیتواند از پایان ان بزرگتر باشد";

                command = ChangePutOn(command);
                command = await FillLists(command);
                return View(command);
            }

            if ( DateTimeConvertor.GetDateFromString(command.StartPutOn) > DateTimeConvertor.GetDateFromString(command.EndPutOn) )
            {
                TempData[SD.Error] = "تاریخ برگذاری مسابقه نمیتواند از اتمام ان بزرگتر باشد";

                command = ChangePutOn(command);
                command = await FillLists(command);
                return View(command);
            }

            step1 = command;

            if ( !command.Id.HasValue )
                return RedirectToAction(nameof(SecondInfo));
            else
                return RedirectToAction(nameof(SecondInfo) , new { id = command.Id.Value });

        }

        private async Task<MatchFirstInfoDto> FillLists ( MatchFirstInfoDto command )
        {
            command.Places = await _placeRepo.GetAllAsync(
                 filter: u => u.ParentPlaceId == null ,
                 select: entity => new SelectListItem { Text = entity.Title , Value = entity.Id.ToString() });

            command.Festivals = await _festivalRepo.GetAllAsync(
                    filter: u => u.Duration.From <= DateTime.Now && u.Duration.To >= DateTime.Now ,
                    select: entity => new SelectListItem { Text = entity.Title , Value = entity.Id.ToString() });

            command.AudienceTypes = await _audRepo.GetAllAsync(select: entity => new SelectListItem { Text = entity.Title , Value = entity.Id.ToString() });

            return command;
        }
        private MatchFirstInfoDto ChangePutOn ( MatchFirstInfoDto command )
        {
            if ( !String.IsNullOrEmpty(command.StartPutOn) && command.StartPutOn.Split(' ').Length >= 9 )
                command.StartPutOn = DateTimeConvertor.GetDateFromString(command.StartPutOn).GetWebToolKitString();

            if ( !String.IsNullOrEmpty(command.EndPutOn) && command.EndPutOn.Split(' ').Length >= 9 )
                command.EndPutOn = DateTimeConvertor.GetDateFromString(command.EndPutOn).GetWebToolKitString();

            if ( !String.IsNullOrEmpty(command.StartRegister) && command.StartRegister.Split(' ').Length >= 9 )
                command.StartRegister = DateTimeConvertor.GetDateFromString(command.StartRegister).GetWebToolKitString();

            if ( !String.IsNullOrEmpty(command.EndRegister) && command.EndRegister.Split(' ').Length >= 9 )
                command.EndRegister = DateTimeConvertor.GetDateFromString(command.EndRegister).GetWebToolKitString();

            return command;
        }

        // step 2
        private static MatchSecondInfoDto? step2;
        public async Task<IActionResult> SecondInfo ( Guid? id , bool readOnly )
        {
            if ( step2 != null && step2.Id == id && !readOnly )
                return View(step2);

            var command = new MatchSecondInfoDto() { ReadOnly = readOnly };
            // Update
            if ( id.HasValue )
            {
                var entity = await _matchRepo.FindAsync(id);
                command.Id = entity.Id;
                command.Image = entity.Image.Name;
                command.Description = entity.Description;
            }

            return View(command);
        }
        [HttpPost]
        public IActionResult SecondInfo ( MatchSecondInfoDto command )
        {
            if ( !ModelState.IsValid )
                return View(command);


            var files = HttpContext.Request.Form.Files;
            if ( !files.Any() && !command.Id.HasValue )
            {
                TempData[SD.Error] = "تصویر مسابقه را وارد کنید";
                return View(command);
            }

            if ( files.Any() )
            {
                command.ImageFile = files[0].ReadBytes();
                command.Image = files[0].FileName;
            }
            step2 = command;


            if ( !command.Id.HasValue )
                return RedirectToAction(nameof(MatchCondition));
            else
                return RedirectToAction(nameof(MatchCondition) , new { id = command.Id.Value });
        }


        // step 3
        private static MatchConditionDto? step3;
        public async Task<IActionResult> MatchCondition ( Guid? id , bool readOnly )
        {
            if ( step3 != null && step3.Id == id && !readOnly )
                return View(step3);

            var command = new MatchConditionDto();
            // update or details
            if ( id.HasValue )
            {
                var entity = await _mcoRepo.FirstOrDefaultAsync(u => u.MatchId == id);
                if ( entity == null )
                {
                    TempData[SD.Error] = "مسابقه وارد شده وجود ندارد";
                    return RedirectToAction(nameof(Index) , _filters);
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
        public IActionResult MatchCondition ( MatchConditionDto command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            var files = HttpContext.Request.Form.Files;
            if ( !files.Any() && !command.Id.HasValue )
            {
                TempData[SD.Error] = "آیین نامه مسابقات را وارد کنید";
                return View(command);
            }

            if ( files.Any() )
            {
                command.RGFile = files[0].ReadBytes();
                command.File = files[0].FileName;
            }
            step3 = command;


            if ( !command.Id.HasValue )
                return RedirectToAction(nameof(MatchDocument));
            else
                return RedirectToAction(nameof(MatchDocument) , new { id = command.Id.Value });
        }


        // step 4
        private static MatchDocumentDto? step4;
        public async Task<IActionResult> MatchDocument ( Guid? id , bool readOnly )
        {
            if ( step4 != null && step4.Id == id && !readOnly )
            {
                step4.Evidences = await _evdRepo.GetAllAsync(select: u => new SelectListItem(u.Title , u.Id.ToString()));
                return View(step4);
            }

            var docs = new MatchDocumentDto() { ReadOnly = readOnly };
            // update
            if ( id.HasValue )
            {
                docs = await _matchRepo.FirstOrDefaultSelectAsync(
                   filter: u => u.Id == id ,
                   select: u => new MatchDocumentDto
                   {
                       Id = id ,
                       Data = "[" + String.Join(',' , u.Documents.Select(u => u.ToJson())) + "]" ,
                       Info = u.Documents.Select(b => new DocumentDataDto { EvidenceId = b.EvidenceId , Type = b.Type })
                   });
            }

            docs.Evidences = await _evdRepo.GetAllAsync(select: u => new SelectListItem(u.Title , u.Id.ToString()));
            docs.ReadOnly = readOnly;
            return View(docs);
        }
        [HttpPost]
        public async Task<IActionResult> MatchDocument ( MatchDocumentDto command )
        {
            command.Info = JsonConvert.DeserializeObject<List<DocumentDataDto>>(command.Data);

            if ( command.Info == null )
            {
                TempData[SD.Error] = "مدارک لازمه را مشخص کنید";
                command.Evidences = await _evdRepo.GetAllAsync(select: u => new SelectListItem(u.Title , u.Id.ToString()));
                return View(command);
            }

            if ( !ModelState.IsValid )
            {
                command.Evidences = await _evdRepo.GetAllAsync(select: u => new SelectListItem(u.Title , u.Id.ToString()));
                return View(command);
            }

            step4 = command;
            if ( !command.Id.HasValue )
                return RedirectToAction(nameof(MatchAward));
            else
                return RedirectToAction(nameof(MatchAward) , new { id = command.Id.Value });
        }


        // step 5
        private static MatchAwardListDto? step5;
        public async Task<IActionResult> MatchAward ( Guid? id , bool readOnly )
        {
            if ( step5 != null && step5.Id == id && !readOnly )
                return View(step5);

            var dto = new MatchAwardListDto() { Id = id , ReadOnly = readOnly };
            // update
            if ( id.HasValue )
            {
                dto.Info = await _matchRepo.FirstOrDefaultSelectAsync(
                        filter: u => u.Id == id.Value ,
                        include: source => source.Include(u => u.Awards) ,
                        select: entity => entity.Awards.Select(u => new MatchAwardDto { Rank = u.Score , Prize = u.Prize }));
            }

            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> MatchAward ( MatchAwardListDto command )
        {
            command.Info = JsonConvert.DeserializeObject<List<MatchAwardDto>>(command.Data);
            if ( command.Info == null )
            {
                TempData[SD.Error] = "جوایز مسابقه را مشخص کنید";
                return View(command);
            }

            step5 = command;

            if ( !command.Id.HasValue )
            {
                await _matchService.CreateAsync(step1 , step2 , step3 , step4 , step5);
                TempData[SD.Success] = "مسابقه با موفقیت ذخیره شد";
            }
            else
            {
                await _matchService.UpdateAsync(step1 , step2 , step3 , step4 , step5);
                TempData[SD.Info] = "ویرایش مسابقه با موفقیت انجام شد";
            }
            step1 = null;
            step2 = null;
            step3 = null;
            step4 = null;
            step5 = null;

            return RedirectToAction(nameof(Index) , _filters);
        }
        #endregion

        [HttpDelete]
        public async Task<JsonResult> Remove ( Guid id )
        {
            await _matchService.RemoveAsync(id);
            return Json(new { Success = true });
        }
    }
}
