using AutoMapper;
using Competitions.Application.Authentication.Interfaces;
using Competitions.Common;
using Competitions.Common.Helpers;
using Competitions.Domain.Dtos.Extracurriculars;
using Competitions.Domain.Entities.Authentication;
using Competitions.Domain.Entities.Extracurriculars;
using Competitions.Web.Models;
using Competitions.Web.Models.Calenders;
using Competitions.Web.Models.Progs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Competitions.Web.Controllers
{
    [Authorize]
    public class ProgController : Controller
    {
        private readonly IRepository<Extracurricular> _extRepo;
        private readonly IRepository<ExtracurricularUser> _extUserRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Role> _roleRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public ProgController(IRepository<Extracurricular> extRepo, IMapper mapper, IRepository<ExtracurricularUser> extUserRepo, IRepository<User> userRepo, IRepository<Role> roleRepo, IPasswordHasher passwordHasher)
        {
            _extRepo = extRepo;
            _mapper = mapper;
            _extUserRepo = extUserRepo;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _passwordHasher = passwordHasher;
        }

        private static ProgCalenderFilter _calenderFilters = new ProgCalenderFilter();
        private static Pagenation _pagenation = new Pagenation();

        public async Task<IActionResult> Index(Pagenation pagenation)
        {
            _pagenation = pagenation;

            pagenation.Total = _extRepo.GetCount();

            if (User.FindFirstValue(ClaimTypes.Actor).ToLower() == "student")
            {
                var vm = new GetAllProgsVM
                {
                    Extracurriculars = await _extRepo.GetAllAsync(
                        filter: u => (
                            (u.Gender.ToString() == User.FindFirstValue(ClaimTypes.Gender) &&
                             u.AudienceType.Title.Contains("دانشجو")) ||
                            User.IsInRole(SD.Admin) || User.IsInRole(SD.Publisher)),
                        include: source => source
                        .Include(u => u.Sport)
                        .Include(u => u.Place)
                        .Include(u => u.AudienceType),
                        orderBy: order => order.OrderByDescending(b => b.Register.To),
                        take: pagenation.Take,
                        skip: pagenation.Skip,
                        select: entity => _mapper.Map<GetExtracurricularDetailsDto>(entity)),
                    Pagenation = pagenation
                };
                return View(vm);
            }
            else
            {
                var vm = new GetAllProgsVM
                {
                    Extracurriculars = await _extRepo.GetAllAsync(
                        include: source => source
                        .Include(u => u.Sport)
                        .Include(u => u.Place)
                        .Include(u => u.AudienceType),
                        orderBy: order => order.OrderByDescending(b => b.Register.To),
                        take: pagenation.Take,
                        skip: pagenation.Skip,
                        select: entity => _mapper.Map<GetExtracurricularDetailsDto>(entity)),
                    Pagenation = pagenation
                };
                return View(vm);
            }
        }

        public IActionResult Calender(ProgCalenderFilter filters)
        {
            filters.ProgDate = _calenderFilters.ProgDate;

            filters.Move();
            filters.RoundMeetingsDate();

            _calenderFilters = filters;
            return View(filters);
        }

        public async Task<JsonResult> CalenderInfo()
        {
            var LastDateOfTheMonth = _calenderFilters.ProgDate.GetTheLastDateOfTheMonth();

            var progs = await _extRepo.GetAllAsync(
                    filter: u =>
                    u.Users.Any(b => b.User.NationalCode.Value == User.FindFirstValue(ClaimTypes.NameIdentifier)) &&
                    u.PutOn.To >= _calenderFilters.ProgDate.Date,
                    include: source => source.Include(u => u.Times));


            var data = new List<CalenderProgData>();
            var startMonth = _calenderFilters.ProgDate;

            while (startMonth.Date != LastDateOfTheMonth.Date)
            {
                foreach (var item in progs)
                {
                    if (item.Times.Any(u => u.Day == startMonth.PersianDayOfWeek()) && item.PutOn.To.Date >= startMonth.Date && item.PutOn.From <= startMonth.Date)
                    {
                        var d = item.Times.First(u => u.Day == startMonth.PersianDayOfWeek()).Time;
                        data.Add(new CalenderProgData(item.Name, new DateTime(startMonth.Year, startMonth.Month, startMonth.Day, d.Hour, d.Min, 0), $"/Prog/Details/{item.Id}"));
                    }
                }
                startMonth = startMonth.AddDays(1);
            }

            return Json(new { Success = true, Data = data });
        }

        public async Task<IActionResult> UserClass(DateTime progDate)
        {
            var extracurriculars = await _extRepo.GetAllAsync(
                filter: u => u.Times.Any(b => b.Day.Equals(progDate.PersianDayOfWeek())) &&
                     u.Users.Any(b => b.User.NationalCode.Value == User.FindFirstValue(ClaimTypes.NameIdentifier)) &&
                    u.PutOn.To.Date >= progDate.Date &&
                    u.Register.From.Date <= progDate.Date,
            include: source => source
            .Include(u => u.Sport)
            .Include(u => u.Place)
            .Include(u => u.AudienceType),
            select: entity => _mapper.Map<GetExtracurricularDetailsDto>(entity));

            return View(extracurriculars);
        }


        public async Task<IActionResult> Details(Guid id)
        {
            var prog = await _extRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id,
                include: source => source
                    .Include(u => u.Coach)
                    .Include(u => u.Times)
                    .Include(u => u.Sport)
                    .Include(u => u.Place)
                    .Include(u => u.AudienceType)
                    .Include(u => u.Users));

            if (prog == null)
            {
                TempData[SD.Error] = "فوق برنامه انتخاب شده در سیستم وجود ندارد";
                return RedirectToAction(nameof(Index), _pagenation);
            }

            return View(prog);
        }


        public async Task<IActionResult> Register(Guid id)
        {
            var prog = await _extRepo.FirstOrDefaultAsync(
                filter: b => b.Id == id,
                include: source => source.Include(b => b.AudienceType));

            if (prog == null)
            {
                TempData[SD.Error] = "فوق برنامه انتخاب شده در سیستم وجود ندارد";
                return RedirectToAction(nameof(Index), _pagenation);
            }

            var count = _extUserRepo.GetCount(b => b.ExtracurricularId == prog.Id);
            if (count == prog.Capacity)
            {
                TempData[SD.Warning] = "ظرفیت فوق برنامه انتخاب شده تکمیل شده است";
                return RedirectToAction(nameof(Details), new { Id = id });
            }

            if (prog.AudienceType.IsNeedInformation)
            {
                return RedirectToAction("RegisterWithInformation", new { Id = id });
            }


            var userId = await _userRepo.FirstOrDefaultSelectAsync(
                filter: u => u.NationalCode.Value == User.FindFirstValue(ClaimTypes.NameIdentifier),
                select: u => u.Id);

            var etc = await _extUserRepo.GetAllAsync(
                filter: b => b.UserId == userId && b.Extracurricular.PutOn.To > DateTime.Now && !b.Extracurricular.Name.Contains("شنا"));


            var userCount = _extUserRepo.GetCount(b => b.UserId == userId && b.ExtracurricularId == id);
            if (userCount > 0)
            {
                TempData[SD.Warning] = "شما در این کلاس عضو هستید";
                return RedirectToAction(nameof(Details), new { Id = id });
            }

            if (etc.Count() == 3)
            {
                TempData[SD.Warning] = "زمان برگذاری کلاس با بقیه کلاس های شما تداخل دارد";
                return RedirectToAction(nameof(Details), new { Id = id });
            }

            var progUser = new ExtracurricularUser(userId, prog.Id, false, null);
            _extUserRepo.Add(progUser);
            await _extRepo.SaveAsync();

            TempData[SD.Success] = "ثبت نام با موفقیت انجام شد";

            return RedirectToAction(nameof(Details), new { Id = id });

        }

        public IActionResult RegisterWithInformation(Guid id)
        {
            var model = new ExtracurricularUserInformationDto { EtcId = id };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterWithInformation(ExtracurricularUserInformationDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var prog = await _extRepo.FirstOrDefaultAsync(
                filter: b => b.Id == model.EtcId,
                include: source => source.Include(b => b.AudienceType));

            if (model.Gender != prog.Gender)
            {
                TempData[SD.Error] = "جنسیت وارد شده با جنسیت کلاس فوق برنامه تداخل دارد";
                return View(model);
            }

            var user = await _userRepo.FirstOrDefaultAsync(b => b.NationalCode.Value == model.NationalCode);

            if (user == null)
            {

                var role = await _roleRepo.FirstOrDefaultAsync(b => b.Title == SD.User);
                user = new User(model.Name, model.Family, model.PhoneNumber, model.NationalCode,
                    model.NationalCode, _passwordHasher.HashPassword(model.NationalCode), role.Id, "000000000", "", model.Gender, "Other");

                _userRepo.Add(user);

            }


            var etc = await _extUserRepo.GetAllAsync(
                filter: b => b.UserId == user.Id && b.Extracurricular.PutOn.To > DateTime.Now && !b.Extracurricular.Name.Contains("شنا"));

            if (etc.Count() == 3)
            {
                TempData[SD.Warning] = $"زمان برگذاری کلاس با بقیه کلاس های {String.Concat(model.Name, ' ', model.Family)} تداخل دارد";
                return RedirectToAction(nameof(Details), new { Id = model.EtcId });
            }

            if (etc.Any(b => b.ExtracurricularId == model.EtcId))
            {
                TempData[SD.Warning] = $"{model.Name} {model.Family} قبلا در کلاس ثبت نام شده است";
                return RedirectToAction(nameof(Details), new { Id = model.EtcId });
            }

            var etcUser = new ExtracurricularUser(user.Id, model.EtcId, model.Insurance, model.Relativity);
            _extUserRepo.Add(etcUser);

            await _userRepo.SaveAsync();

            TempData[SD.Success] = $"{model.Name} {model.Family} با موفقیت در کلاس ثبت نام شد";
            return RedirectToAction(nameof(Details), new { Id = model.EtcId });
        }


    }
}
