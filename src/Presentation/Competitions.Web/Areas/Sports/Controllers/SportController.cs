using AutoMapper;
using Competitions.Application;
using Competitions.Application.Sports.Interfaces;
using Competitions.Common;
using Competitions.Common.Helpers;
using Competitions.Domain.Dtos.Sports.Sports;
using Competitions.Domain.Entities.Sports;
using Competitions.Domain.Entities.Sports.Spec;
using Competitions.SharedKernel.ValueObjects;
using Competitions.Web.Areas.Sports.Models.Sports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Competitions.Web.Areas.Sports.Controllers
{
    [Area("Sports")]
    public class SportController : Controller
    {
        private readonly IRepository<Sport> _sportRepo;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly ISportTypeService _sportTypeService;
        private readonly IMapper _mapper;

        private static SportFilter _filters = new SportFilter();

        public SportController ( IRepository<Sport> sportRepo , IWebHostEnvironment hostEnv , ISportTypeService sportTypeService , IMapper mapper )
        {
            _sportRepo = sportRepo;
            _hostEnv = hostEnv;
            _sportTypeService = sportTypeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index ( SportFilter filters )
        {
            _filters = filters;
            filters.CreateDate = filters.CreateDate.ToMiladi();
            filters.Types = _sportTypeService.GetSelectedList();

            var spec = new GetFilteredSportsSpec(filters.Name , filters.CreateDate , filters.SportTypeId , filters.Skip , filters.Take);

            var vm = new GetAllSportsVM
            {
                Entities = _sportRepo.GetAll(spec , entity => _mapper.Map<GetSportDto>(entity)) ,
                Filters = filters ,
            };

            filters.Total = _sportRepo.GetCount(spec);

            return View(vm);
        }


        public async Task<IActionResult> Details ( long id )
        {
            var entity = await _sportRepo.FirstOrDefaultAsync(filter: u => u.Id == id , include: source => source.Include(u => u.SportType));
            if ( entity == null )
            {
                TempData[SD.Error] = "رشته ورزشی انتخاب شده وجود  ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            return View(entity);
        }


        public IActionResult Create ()
        {
            var dto = new CreateSportDto { SportTypes = _sportTypeService.GetSelectedList() };
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Create ( CreateSportDto command )
        {
            var files = HttpContext.Request.Form.Files;

            if ( !ModelState.IsValid || files.Count() == 0 )
            {
                command.SportTypes = _sportTypeService.GetSelectedList();
                return View(command);
            }

            var entity = new Sport(command.Name , command.Description ,
                new Document(files[0].FileName , files[0].ReadBytes()) , command.SportTypeId);
            entity.SaveImage();

            _sportRepo.Add(entity);
            await _sportRepo.SaveAsync();

            TempData[SD.Success] = "رشته ورزشی با موفقیت ذخیره شد";
            return RedirectToAction(nameof(Index) , _filters);
        }


        public async Task<IActionResult> Update ( long id )
        {
            var entity = await _sportRepo.FindAsync(id);
            if ( entity == null )
            {
                TempData[SD.Error] = "رشته ورزشی انتخاب شده وجود  ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            var dto = _mapper.Map<UpdateSportDto>(entity);
            dto.SportTypes = _sportTypeService.GetSelectedList();

            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateSportDto command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            var entity = await _sportRepo.FindAsync(command.Id);
            var files = HttpContext.Request.Form.Files;

            if ( files.Any() )
            {
                entity.DeleteImage();
                entity.WithImage(new Document(files[0].FileName , files[0].ReadBytes()))
                    .SaveImage();
            }

            entity.WithName(command.Name)
                .WithDescription(command.Description)
                .WithSportTypeId(command.SportTypeId);

            _sportRepo.Update(entity);
            await _sportRepo.SaveAsync();

            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index) , _filters);
        }


        public async Task<IActionResult> Remove ( long id )
        {
            var entity = await _sportRepo.FindAsync(id);

            if ( entity == null )
                return Json(new { Success = false });

            entity.DeleteImage();
            _sportRepo.Remove(entity);
            await _sportRepo.SaveAsync();

            return Json(new { Success = true });
        }



        private async Task<IEnumerable<GetSportDto>> GetFilteredList () =>
            await _sportRepo.GetAllAsync<GetSportDto>(
                filter: entity => entity.Name.Equals(_filters.Name) && entity.CreateDate.Date.Equals(_filters.CreateDate.Value.Date) &&
                entity.SportTypeId.Equals(_filters.SportTypeId.Value) ,
                take: _filters.Take ,
                skip: _filters.Skip ,
                orderBy: source => source.OrderByDescending(u => u.CreateDate) ,
                select: entity => _mapper.Map<GetSportDto>(entity));

        private int GetFilteredCount () =>
            _sportRepo.GetCount(
                filter: entity => ( !_filters.CreateDate.HasValue || entity.CreateDate.Date.Equals(_filters.CreateDate.Value.Date) ) &&
                        ( String.IsNullOrEmpty(_filters.Name) || entity.Name.Equals(_filters.Name) ));

    }
}
