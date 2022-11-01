using AutoMapper;
using Competitions.Application.Sports.Interfaces;
using Competitions.Common;
using Competitions.Domain.Dtos.Sports.Coaches;
using Competitions.Domain.Entities.Sports;
using Competitions.Domain.Entities.Sports.Spec;
using Competitions.Web.Areas.Sports.Models.Coaches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Competitions.Web.Areas.Sports.Controllers
{
    [Area("Sports")]
    [Authorize($"{SD.Publisher},{SD.Admin}")]
    public class CoachController : Controller
    {
        private readonly IRepository<Coach> _coachRepo;
        private readonly ICETService _cetService;
        private readonly ISportService _sportService;
        private readonly IMapper _mapper;

        private static CoachFilter _filters = new CoachFilter();

        public CoachController ( IRepository<Coach> coachRepo , ICETService cetService , ISportService sportSrvice , IMapper mapper )
        {
            _coachRepo = coachRepo;
            _cetService = cetService;
            _sportService = sportSrvice;
            _mapper = mapper;
        }

        public IActionResult Index ( CoachFilter filters )
        {
            _filters = filters;

            filters.CETs = _cetService.GetSelectedList();
            filters.Sports = _sportService.GetSelectedList();

            var spec = new GetFilteredCoachSpec(filters.Name , filters.Family , filters.Education ,
                filters.NationalCode , filters.SportId , filters.CETId , filters.Skip , filters.Take);

            filters.Total = _coachRepo.GetCount(spec);
            var vm = new GetAllCoaches
            {
                Coaches = _coachRepo.GetAll<GetCoachDto>(spec , entity => _mapper.Map<GetCoachDto>(entity)) ,
                Filters = filters
            };

            return View(vm);
        }


        public async Task<IActionResult> Details ( long id )
        {
            var entity = await _coachRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source
                    .Include(u => u.Sport)
                    .Include(u => u.CoachEvidenceType));

            if ( entity == null )
            {
                TempData[SD.Error] = "مربی انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            return View(entity);
        }


        public IActionResult Create ()
        {
            var dto = new CreateCoachDto { Sports = _sportService.GetSelectedList() , CETs = _cetService.GetSelectedList() };
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Create ( CreateCoachDto command )
        {
            if ( !ModelState.IsValid )
            {
                command.Sports = _sportService.GetSelectedList();
                command.CETs = _cetService.GetSelectedList();
                return View(command);
            }

            var entity = command.ToEntity();
            _coachRepo.Add(entity);
            await _coachRepo.SaveAsync();

            TempData[SD.Success] = "مربی با موفقیت اضافه شد";
            return RedirectToAction(nameof(Index) , _filters);
        }



        public IActionResult Update ( long id )
        {
            var entity = _coachRepo.Find(id);
            if ( entity == null )
            {
                TempData[SD.Error] = "مربی انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            var dto = _mapper.Map<UpdateCoachDto>(entity);
            dto.Sports = _sportService.GetSelectedList();
            dto.CETs = _cetService.GetSelectedList();

            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateCoachDto command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            Coach entity = _coachRepo.Find(command.Id);
            entity = command.UpdateEntity(entity);

            _coachRepo.Update(entity);
            await _coachRepo.SaveAsync();

            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index) , _filters);
        }



        [HttpDelete]
        public async Task<IActionResult> Remove ( long id )
        {
            var entity = await _coachRepo.FindAsync(id);
            if ( entity == null )
                return Json(new { Success = false });

            _coachRepo.Remove(entity);
            await _coachRepo.SaveAsync();

            return Json(new { Success = true });
        }

    }
}
