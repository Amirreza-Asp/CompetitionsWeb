using AutoMapper;
using ClosedXML.Excel;
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
        private readonly IWebHostEnvironment _hostEnv;

        private static ExtracurricularFilter _filters = new ExtracurricularFilter();

        public ExtracurricularController(IRepository<Extracurricular> extRepo, IRepository<Sport> sportRepo, IRepository<AudienceType> audRepo, IRepository<Place> placeRepo, IMapper mapper, IRepository<ExtracurricularTime> extTimeRepo, IRepository<ExtracurricularUser> extUserRepo, IWebHostEnvironment hostEnv)
        {
            _extRepo = extRepo;
            _sportRepo = sportRepo;
            _audRepo = audRepo;
            _placeRepo = placeRepo;
            _mapper = mapper;
            _extTimeRepo = extTimeRepo;
            _extUserRepo = extUserRepo;
            _hostEnv = hostEnv;
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

            if (command.StartRegister.ToDateTime() >= command.EndRegister.ToDateTime())
            {
                command.Places = await _placeRepo.GetAllAsync(u => u.ParentPlaceId == null,
                    select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });
                command.AudienceTypes = await _audRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });


                TempData[SD.Error] = "زمان شروع ثبت نام نمیتواند از پایان بزرگتر باشد";
                return View(command);
            }

            if (command.StartPutOn.ToDateTime() >= command.EndPutOn.ToDateTime())
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
                new DateTimeRange(command.StartPutOn.ToDateTime(), command.EndPutOn.ToDateTime()), command.Gender, new DateTimeRange(command.StartRegister.ToDateTime(), command.EndRegister.ToDateTime()), command.Description,
                command.CoachId, command.MinimumPlacements, command.IsFree);

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

            if (command.StartRegister.ToDateTime() >= command.EndRegister.ToDateTime())
            {
                command.Places = await _placeRepo.GetAllAsync(u => u.ParentPlaceId == null,
                    select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });
                command.AudienceTypes = await _audRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });


                TempData[SD.Error] = "زمان شروع ثبت نام نمیتواند از پایان بزرگتر باشد";
                return View(command);
            }

            if (command.StartPutOn.ToDateTime() >= command.EndPutOn.ToDateTime())
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
                .WithIsFree(command.IsFree)
                .WithCoachId(command.CoachId)
                .WithPlaceId(command.PlaceId)
                .WithPutOn(new DateTimeRange(command.StartPutOn.ToDateTime(), command.EndPutOn.ToDateTime()))
                .WithRegister(new DateTimeRange(command.StartRegister.ToDateTime(), command.EndRegister.ToDateTime()))
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

        public async Task<IActionResult> PrintExcel(Guid extraId)
        {
            var users =
                await _extUserRepo.GetAllAsync(
                    user => user.ExtracurricularId == extraId,
                    include: source => source.Include(u => u.User),
                    orderBy: user => user
                        .OrderBy(b => b.User.Name)
                        .OrderBy(b => b.User.Family));

            var extName = await _extRepo.FirstOrDefaultAsync(b => b.Id == extraId);


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Students");

                var currentRow = 1;

                #region header
                worksheet.Cell(currentRow, 1).Value = "نام";
                worksheet.Cell(currentRow, 2).Value = "کد ملی";
                worksheet.Cell(currentRow, 3).Value = "شماره دانشجویی";
                worksheet.Cell(currentRow, 4).Value = "رشته تحصیلی";
                worksheet.Cell(currentRow, 5).Value = "شماره تلفن";
                worksheet.Cell(currentRow, 6).Value = "بیمه ورزشی";
                worksheet.Cell(currentRow, 7).Value = "تاریخ ثبت نام";
                #endregion

                #region body
                foreach (var extUser in users)
                {
                    currentRow++;

                    worksheet.Cell(currentRow, 1).Value = extUser.User.Name + " " + extUser.User.Family;
                    worksheet.Cell(currentRow, 2).Value = extUser.User.NationalCode.Value;
                    worksheet.Cell(currentRow, 3).Value = extUser.User.StudentNumber.Value == "000000000" ? "ندارد" : extUser.User.StudentNumber.Value;
                    worksheet.Cell(currentRow, 4).Value = extUser.User.College == null ? "خارج از دانشگاه" : extUser.User.College;
                    worksheet.Cell(currentRow, 5).Value = extUser.User.PhoneNumber == null ? "" : extUser.User.PhoneNumber.Value;
                    worksheet.Cell(currentRow, 6).Value = extUser.Insurance ? "دارد" : "ندارد";
                    worksheet.Cell(currentRow, 7).Value = extUser.JoinTime.ToShamsi();
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

        [HttpDelete]
        public async Task<IActionResult> RemoveStudent(Guid userId, Guid extId)
        {
            var extUser = await _extUserRepo.FirstOrDefaultAsync(b => b.UserId == userId && b.ExtracurricularId == extId);
            if (extUser == null)
                return Json(new { Success = false, Message = "فرد مورد نظر در کلاس وجود ندارد" });

            _extUserRepo.Remove(extUser);
            await _extUserRepo.SaveAsync();

            return Json(new { Success = true, Message = "با موفقیت از کلاس حذف شد" });
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
