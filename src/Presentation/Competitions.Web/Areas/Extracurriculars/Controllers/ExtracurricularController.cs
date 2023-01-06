using AutoMapper;
using Competitions.Common;
using Competitions.Common.Helpers;
using Competitions.Domain.Dtos.Extracurriculars;
using Competitions.Domain.Entities.Extracurriculars;
using Competitions.Domain.Entities.Extracurriculars.Spec;
using Competitions.Domain.Entities.Managment.ValueObjects;
using Competitions.Domain.Entities.Places;
using Competitions.Domain.Entities.Sports;
using Competitions.Domain.Entities.Static;
using Competitions.Web.Areas.Managment.Models.Extracurriculars;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Competitions.Web.Areas.Extracurriculars.Controllers
{
    [Area("Extracurriculars")]
    [Authorize(Roles = $"{SD.Publisher},{SD.Admin}")]
    public class ExtracurricularController : Controller
    {
        private readonly IRepository<Extracurricular> _extRepo;
        private readonly IRepository<ExtracurricularUser> _extUserRepo;
        private readonly IRepository<ExtracurricularTime> _extTimeRepo;
        private readonly IRepository<Sport> _sportRepo;
        private readonly IRepository<AudienceType> _audRepo;
        private readonly IRepository<Place> _placeRepo;
        private readonly IMapper _mapper;

        private static ExtracurricularFilter _filters = new ExtracurricularFilter();

        public ExtracurricularController(IRepository<Extracurricular> extRepo, IRepository<Sport> sportRepo, IRepository<AudienceType> audRepo, IRepository<Place> placeRepo, IMapper mapper, IRepository<ExtracurricularTime> extTimeRepo, IRepository<ExtracurricularUser> extUserRepo)
        {
            _extRepo = extRepo;
            _sportRepo = sportRepo;
            _audRepo = audRepo;
            _placeRepo = placeRepo;
            _mapper = mapper;
            _extTimeRepo = extTimeRepo;
            _extUserRepo = extUserRepo;
        }

        public async Task<IActionResult> Index(ExtracurricularFilter filters)
        {
            if (_filters.AudienceTypes == null)
            {
                filters.AudienceTypes = await _audRepo.GetAllAsync(select: entity => new SelectListItem { Text = entity.Title, Value = entity.Id.ToString() });
                filters.Sports = await _sportRepo.GetAllAsync(select: entity => new SelectListItem { Text = entity.Name, Value = entity.Id.ToString() });
                filters.Places = await _placeRepo.GetAllAsync(select: entity => new SelectListItem { Text = entity.Title, Value = entity.Id.ToString() });

            }
            else
            {
                filters.AudienceTypes = _filters.AudienceTypes;
                filters.Sports = _filters.Sports;
                filters.Places = _filters.Places;
            }

            _filters = filters;

            var spec = new GetFilteredExtracurricularSpec(filters.Name, filters.SportId, filters.AudienceTypeId, filters.PlaceId, filters.Skip, filters.Take);
            filters.Total = _extRepo.GetCount(spec);

            var vm = new GetAllExtracurricularsVM
            {
                Extracurriculars = _extRepo.GetAll(spec, entity => _mapper.Map<GetExtracurricularDetailsDto>(entity)),
                Filter = filters
            };

            return View(vm);
        }


        public async Task<IActionResult> Details(Guid id)
        {
            var entity = await _extRepo
                .FirstOrDefaultAsync(
                filter: u => u.Id == id,
                include: source =>
                    source.Include(u => u.Times)
                    .Include(u => u.Sport)
                    .Include(u => u.Place)
                    .Include(u => u.AudienceType)
                    .Include(u => u.Coach));

            if (entity == null)
            {
                TempData[SD.Error] = "فوق برنامه انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index), _filters);
            }

            return View(entity);
        }


        public async Task<IActionResult> Create()
        {
            var dto = new CreateExtracurricularDto
            {
                Places = await _placeRepo.GetAllAsync(u => u.ParentPlaceId == null, select: u => new SelectListItem
                {
                    Text = u.Title,
                    Value = u.Id.ToString()
                }),

                AudienceTypes = await _audRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() })
            };

            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateExtracurricularDto command)
        {

            command.StartPutOn = command.StartPutOn.ToMiladi();
            command.EndPutOn = command.EndPutOn.ToMiladi();
            command.StartRegister = command.StartRegister.ToMiladi();
            command.EndRegister = command.EndRegister.ToMiladi();

            if (!ModelState.IsValid)
            {
                command.Places = await _placeRepo.GetAllAsync(u => u.ParentPlaceId == null,
                    select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });
                command.AudienceTypes = await _audRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });


                return View(command);
            }

            if (command.StartRegister >= command.EndRegister)
            {
                command.Places = await _placeRepo.GetAllAsync(u => u.ParentPlaceId == null,
                    select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });
                command.AudienceTypes = await _audRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });


                TempData[SD.Error] = "زمان شروع ثبت نام نمیتواند از پایان بزرگتر باشد";
                return View(command);
            }

            if (command.StartPutOn >= command.EndPutOn)
            {
                command.Places = await _placeRepo.GetAllAsync(u => u.ParentPlaceId == null,
               select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });
                command.AudienceTypes = await _audRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });


                TempData[SD.Error] = "زمان شروع دوره نمیتواند از پایان ان بزرگتر باشد";
                return View(command);
            }

            if (command.MinimumPlacements > command.Capacity)
            {
                command.Places = await _placeRepo.GetAllAsync(u => u.ParentPlaceId == null,
               select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });
                command.AudienceTypes = await _audRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });

                TempData[SD.Error] = "حداقل ظرفیت برای برگزاری دوره باید از ظرفیت دوره کمتر باشد";
                return View(command);
            }

            var entity = new Extracurricular(command.Name, command.SportId, command.PlaceId, command.AudienceTypeId, command.Capacity,
                new DateTimeRange(command.StartPutOn, command.EndPutOn), command.Gender, new DateTimeRange(command.StartRegister, command.EndRegister), command.Description,
                command.CoachId, command.MinimumPlacements);

            var times = JsonConvert.DeserializeObject<List<ExtracurricularTimeDto>>(command.Times);

            if (times != null)
                times.ForEach(time =>
                {
                    entity.AddTime(new ExtracurricularTime(time.Day, new Time(time.GetHour(), time.GetMin()), entity.Id));
                });

            _extRepo.Add(entity);
            await _extRepo.SaveAsync();

            TempData[SD.Success] = "فوق برنامه با موفقیت ذخیره شد";
            return RedirectToAction(nameof(Index), _filters);
        }


        public async Task<IActionResult> Update(Guid id)
        {
            var entity = await _extRepo.FirstOrDefaultAsync(u => u.Id == id, include: source => source.Include(u => u.Times));
            if (entity == null)
            {
                TempData[SD.Error] = "فوق برنامه انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index), _filters);
            }

            var dto = _mapper.Map<UpdateExtracurricularDto>(entity);

            dto.Places = await _placeRepo.GetAllAsync(u => u.ParentPlaceId == null, select: u => new SelectListItem
            { Text = u.Title, Value = u.Id.ToString() });
            dto.AudienceTypes = await _audRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateExtracurricularDto command)
        {

            command.StartPutOn = command.StartPutOn.ToMiladi();
            command.EndPutOn = command.EndPutOn.ToMiladi();
            command.StartRegister = command.StartRegister.ToMiladi();
            command.EndRegister = command.EndRegister.ToMiladi();

            if (!ModelState.IsValid)
            {
                command.Places = await _placeRepo.GetAllAsync(u => u.ParentPlaceId == null,
                    select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });
                command.AudienceTypes = await _audRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });


                return View(command);
            }

            if (command.StartRegister >= command.EndRegister)
            {
                command.Places = await _placeRepo.GetAllAsync(u => u.ParentPlaceId == null,
                    select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });
                command.AudienceTypes = await _audRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });


                TempData[SD.Error] = "زمان شروع ثبت نام نمیتواند از پایان بزرگتر باشد";
                return View(command);
            }

            if (command.StartPutOn >= command.EndPutOn)
            {
                command.Places = await _placeRepo.GetAllAsync(u => u.ParentPlaceId == null,
               select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });
                command.AudienceTypes = await _audRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });


                TempData[SD.Error] = "زمان شروع دوره نمیتواند از پایان ان بزرگتر باشد";
                return View(command);
            }

            if (command.MinimumPlacements > command.Capacity)
            {
                command.Places = await _placeRepo.GetAllAsync(u => u.ParentPlaceId == null,
               select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });
                command.AudienceTypes = await _audRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });

                TempData[SD.Error] = "حداقل ظرفیت برای برگزاری دوره باید از ظرفیت دوره کمتر باشد";
                return View(command);
            }

            var entity = await _extRepo.FirstOrDefaultAsync(u => u.Id == command.Id, include: source => source.Include(u => u.Times));

            var times = JsonConvert.DeserializeObject<List<ExtracurricularTimeDto>>(command.Times);

            if (times != null)
            {
                entity.Times.ToList().ForEach(time => _extTimeRepo.Remove(time));

                foreach (var time in times)
                    _extTimeRepo.Add(new ExtracurricularTime(time.Day.Trim(), time.Time.Trim(), entity.Id));
            }


            entity.WithGender(command.Gender)
                .WithCapacity(command.Capacity)
                .WithDescripion(command.Description)
                .WithSportId(command.SportId)
                .WithCoachId(command.CoachId)
                .WithPlaceId(command.PlaceId)
                .WithPutOn(new DateTimeRange(command.StartPutOn, command.EndPutOn))
                .WithRegister(new DateTimeRange(command.StartRegister, command.EndRegister))
                .WithAudienceTypeId(command.AudienceTypeId)
                .WithMinimumPlacements(command.MinimumPlacements)
                .WithName(command.Name);

            _extRepo.Update(entity);
            await _extRepo.SaveAsync();

            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index), _filters);
        }

        public async Task<IActionResult> Remove(Guid id)
        {
            var entity = await _extRepo.FindAsync(id);
            if (entity == null)
                return Json(new { Success = false });

            _extRepo.Remove(entity);
            await _extRepo.SaveAsync();
            return Json(new { Success = true });
        }

        public async Task<IActionResult> Students(Guid extraId)
        {
            var users =
                await _extUserRepo.GetAllAsync(
                    user => user.ExtracurricularId == extraId,
                    include: source => source.Include(u => u.User),
                    orderBy: user => user
                        .OrderBy(b => b.User.Name)
                        .OrderBy(b => b.User.Family));

            return View(users);
        }


        public async Task<JsonResult> GetSportsByPlaceId(Guid id)
        {
            var sports = await _placeRepo.FirstOrDefaultSelectAsync(
                filter: u => u.Id == id,
                include: source => source.Include(u => u.Sports).ThenInclude(u => u.Sport),
                select: s => s.Sports.Select(u => new SelectListItem { Text = u.Sport.Name, Value = u.Sport.Id.ToString() }));

            return Json(new { Exists = true, data = sports });
        }

        public async Task<JsonResult> GetSubPlacesByPlaceId(Guid id)
        {
            var subPlaces = await _placeRepo.GetAllAsync(u => u.ParentPlaceId == id, select: u => new SelectListItem
            {
                Text = u.Title,
                Value = u.Id.ToString()
            });

            if (subPlaces.Any())
                return Json(new { Exists = true, data = subPlaces });

            return Json(new { Exists = true, data = new List<SelectListItem> { new SelectListItem { Text = "", Value = id.ToString() } } });
        }

        public async Task<JsonResult> GetCoachBySportId(long id)
        {
            var coaches = await _sportRepo.FirstOrDefaultSelectAsync(
                filter: u => u.Id == id,
                include: source => source.Include(u => u.Coaches),
                select: entity => entity.Coaches.Select(u => new SelectListItem { Text = u.Name + " " + u.Family, Value = u.Id.ToString() }));

            if (coaches.Any())
                return Json(new { Exists = true, data = coaches });
            return Json(new { Exists = false });
        }

        public async Task<JsonResult> GetParentPlaceByChildPlaceId(Guid id)
        {
            if (id == default)
                return Json(new { Exists = false });

            var parent = await _placeRepo.FirstOrDefaultSelectAsync(
                    filter: u => u.Id == id,
                    include: source => source.Include(u => u.ParentPlace),
                    select: entity => new SelectListItem { Text = entity.ParentPlace.Title, Value = entity.ParentPlace.Id.ToString() });

            if (string.IsNullOrEmpty(parent.Value))
            {
                parent = await _placeRepo.FirstOrDefaultSelectAsync(
                    filter: u => u.Id == id,
                    select: entity => new SelectListItem { Text = entity.Title, Value = entity.Id.ToString() });
            }

            return Json(new { Exists = true, data = parent });
        }

    }
}
