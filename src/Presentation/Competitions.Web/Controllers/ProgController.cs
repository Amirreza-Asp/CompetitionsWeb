using AutoMapper;
using Competitions.Common;
using Competitions.Common.Helpers;
using Competitions.Domain.Dtos.Managment.Extracurriculars;
using Competitions.Domain.Entities.Authentication;
using Competitions.Domain.Entities.Managment;
using Competitions.Web.Models;
using Competitions.Web.Models.Calenders;
using Competitions.Web.Models.Progs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Competitions.Web.Controllers
{
    public class ProgController : Controller
    {
        private readonly IRepository<Extracurricular> _extRepo;
        private readonly IRepository<ExtracurricularUser> _extUserRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IMapper _mapper;

        public ProgController ( IRepository<Extracurricular> extRepo , IMapper mapper , IRepository<ExtracurricularUser> extUserRepo , IRepository<User> userRepo )
        {
            _extRepo = extRepo;
            _mapper = mapper;
            _extUserRepo = extUserRepo;
            _userRepo = userRepo;
        }

        private static ProgCalenderFilter _calenderFilters = new ProgCalenderFilter();
        private static Pagenation _pagenation = new Pagenation();

        public async Task<IActionResult> Index ( Pagenation pagenation )
        {
            _pagenation = pagenation;

            pagenation.Total = _extRepo.GetCount();
            var vm = new GetAllProgsVM
            {
                Extracurriculars = await _extRepo.GetAllAsync(
                    include: source => source
                    .Include(u => u.Sport)
                    .Include(u => u.Place)
                    .Include(u => u.AudienceType) ,
                    take: pagenation.Take ,
                    skip: pagenation.Skip ,
                    select: entity => _mapper.Map<GetExtracurricularDetailsDto>(entity)) ,
                Pagenation = pagenation
            };
            return View(vm);
        }

        public IActionResult Calender ( ProgCalenderFilter filters )
        {
            filters.ProgDate = _calenderFilters.ProgDate;

            filters.Move();
            filters.RoundMeetingsDate();

            _calenderFilters = filters;
            return View(filters);
        }

        public async Task<JsonResult> CalenderInfo ()
        {
            var LastDateOfTheMonth = _calenderFilters.ProgDate.GetTheLastDateOfTheMonth();

            var progs = await _extRepo.GetAllAsync(
                    filter: u =>
                    u.Users.Any(b => b.User.NationalCode.Value == User.FindFirstValue(ClaimTypes.NameIdentifier)) &&
                    u.PutOn.To >= _calenderFilters.ProgDate.Date ,
                    include: source => source.Include(u => u.Times));


            var data = new List<CalenderProgData>();
            var startMonth = _calenderFilters.ProgDate;

            while ( startMonth.Date != LastDateOfTheMonth.Date )
            {
                foreach ( var item in progs )
                {
                    if ( item.Times.Any(u => u.Day == startMonth.PersianDayOfWeek()) && item.PutOn.To.Date >= startMonth.Date && item.PutOn.From <= startMonth.Date )
                    {
                        var d = item.Times.First(u => u.Day == startMonth.PersianDayOfWeek()).Time;
                        data.Add(new CalenderProgData(item.Name , new DateTime(startMonth.Year , startMonth.Month , startMonth.Day , d.Hour , d.Min , 0) , $"/Prog/Details/{item.Id}"));
                    }
                }
                startMonth = startMonth.AddDays(1);
            }

            return Json(new { Success = true , Data = data });
        }

        public async Task<IActionResult> UserClass ( DateTime progDate )
        {
            var extracurriculars = await _extRepo.GetAllAsync(
                filter: u => u.Times.Any(b => b.Day.Equals(progDate.PersianDayOfWeek())) &&
                     u.Users.Any(b => b.User.NationalCode.Value == User.FindFirstValue(ClaimTypes.NameIdentifier)) &&
                    u.PutOn.To.Date >= progDate.Date &&
                    u.Register.From.Date <= progDate.Date ,
            include: source => source
            .Include(u => u.Sport)
            .Include(u => u.Place)
            .Include(u => u.AudienceType) ,
            select: entity => _mapper.Map<GetExtracurricularDetailsDto>(entity));

            return View(extracurriculars);
        }


        public async Task<IActionResult> Details ( Guid id )
        {
            var prog = await _extRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source
                    .Include(u => u.Coach)
                    .Include(u => u.Times)
                    .Include(u => u.Sport)
                    .Include(u => u.Place)
                    .Include(u => u.AudienceType));

            if ( prog == null )
            {
                TempData[SD.Error] = "فوق برنامه انتخاب شده در سیستم وجود ندارد";
                return RedirectToAction(nameof(Index) , _pagenation);
            }

            return View(prog);
        }


        public async Task<IActionResult> Register ( Guid id )
        {
            var prog = await _extRepo.FindAsync(id);

            if ( prog == null )
            {
                TempData[SD.Error] = "فوق برنامه انتخاب شده در سیستم وجود ندارد";
                return RedirectToAction(nameof(Index) , _pagenation);
            }

            var count = _extUserRepo.GetCount(b => b.ExtracurricularId == prog.Id);
            if ( count == prog.Capacity )
            {
                TempData[SD.Warning] = "ظرفیت فوق برنامه انتخاب شده تکمیل شده است";
                return RedirectToAction(nameof(Details) , new { Id = id });
            }


            var userId = await _userRepo.FirstOrDefaultSelectAsync(
                filter: u => u.NationalCode.Value == User.FindFirstValue(ClaimTypes.NameIdentifier) ,
                select: u => u.Id);


            var userCount = _extUserRepo.GetCount(b => b.UserId == userId && b.ExtracurricularId == id);
            if ( userCount > 0 )
            {
                TempData[SD.Warning] = "شما در این کلاس عضو هستید";
                return RedirectToAction(nameof(Details) , new { Id = id });
            }

            var progUser = new ExtracurricularUser(userId , prog.Id);
            _extUserRepo.Add(progUser);
            await _extRepo.SaveAsync();

            TempData[SD.Success] = "ثبت نام با موفقیت انجام شد";

            return RedirectToAction(nameof(Details) , new { Id = id });

        }
    }
}
