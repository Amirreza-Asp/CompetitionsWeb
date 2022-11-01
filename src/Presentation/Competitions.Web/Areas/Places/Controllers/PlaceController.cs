using AutoMapper;
using Competitions.Application.Places.Interfaces;
using Competitions.Common;
using Competitions.Common.Helpers;
using Competitions.Domain.Dtos.Places.ActivityPlans;
using Competitions.Domain.Dtos.Places.Image;
using Competitions.Domain.Dtos.Places.Places;
using Competitions.Domain.Entities;
using Competitions.Domain.Entities.Places;
using Competitions.Domain.Entities.Places.Spec;
using Competitions.Domain.Entities.Sports;
using Competitions.SharedKernel.ValueObjects;
using Competitions.Web.Areas.Places.Models.Places;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Competitions.Web.Areas.Places.Controllers
{
    [Area("Places")]
    [Authorize($"{SD.Publisher},{SD.Admin}")]
    public class PlaceController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Place> _placeRepo;
        private readonly IRepository<PlaceType> _placeTypeRepo;
        private readonly IRepository<Supervisor> _supervisorRepo;
        private readonly IRepository<PlaceImages> _placeImageRepo;
        private readonly IRepository<Sport> _sportRepo;
        private readonly IPlaceService _placeService;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IRepository<ActivityPlan> _activityPlanRepo;

        private static PlaceFilter _filters = new PlaceFilter();

        public PlaceController ( IMapper mapper , IRepository<Place> placeRepo , IRepository<PlaceType> placeTypeRepo , IRepository<Supervisor> supervisorRepo , IRepository<PlaceImages> placeImageRepo , IRepository<Sport> sportRepo , IPlaceService placeService , IWebHostEnvironment hostEnv , IRepository<ActivityPlan> activityPlanRepo )
        {
            _mapper = mapper;
            _placeRepo = placeRepo;
            _placeTypeRepo = placeTypeRepo;
            _supervisorRepo = supervisorRepo;
            _placeImageRepo = placeImageRepo;
            _sportRepo = sportRepo;
            _placeService = placeService;
            _hostEnv = hostEnv;
            _activityPlanRepo = activityPlanRepo;
        }

        public IActionResult Index ( PlaceFilter filters )
        {
            _filters = filters;

            var spec = new GetFilteredPlacesSpec(filters.PlaceTypeId , filters.Title , filters.SportId , filters.Skip , filters.Take);

            filters.Total = _placeRepo.GetCount(spec);
            var vm = new GetAllPlacesVM
            {
                Places = _placeRepo.GetAll(spec , select: entity => _mapper.Map<GetPlaceDto>(entity)) ,
                Filters = filters
            };

            return View(vm);
        }


        public async Task<IActionResult> Details ( Guid id )
        {
            var place = await _placeRepo.FirstOrDefaultAsync(
               filter: u => u.Id == id ,
               include: source => source.Include(u => u.Supervisor)
               .Include(u => u.PlaceType)
               .Include(u => u.ParentPlace)
               .Include(u => u.Images)
               .Include(u => u.Sports)
                   .ThenInclude(u => u.Sport));

            if ( place == null )
            {
                TempData[SD.Error] = "مکان انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            return View(place);
        }

        public async Task<IActionResult> Create ()
        {
            var dto = new CreatePlaceDto();
            dto = await FillPlaceInfo(dto);
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Create ( CreatePlaceDto command )
        {
            if ( !ModelState.IsValid )
            {
                command = await FillPlaceInfo(command);
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            if ( files.Any() )
            {
                foreach ( var file in files )
                {
                    if ( file.Length > SD.ImageSizeLimit )
                    {
                        command = await FillPlaceInfo(command);
                        TempData[SD.Error] = $"سایز عکس وارد شده باید کمتر از {SD.ImageSizeLimitDisplay} باشد";
                        return View(command);
                    }
                }
            }

            await _placeService.CreateAsync(command , files);
            TempData[SD.Success] = "مکان با موفقیت ذخیره شد";
            return RedirectToAction(nameof(Index) , _filters);
        }


        public async Task<IActionResult> Update ( Guid id )
        {
            var place = await _placeRepo.FirstOrDefaultSelectAsync<UpdatePlaceDto>(
                filter: u => u.Id == id ,
                include: source => source.Include(u => u.Supervisor)
                .Include(u => u.Images)
                .Include(u => u.Sports)
                    .ThenInclude(u => u.Sport) ,
                select: entity => _mapper.Map<UpdatePlaceDto>(entity));

            if ( place == null )
            {
                TempData[SD.Error] = "مکان انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            place = ( UpdatePlaceDto ) await FillPlaceInfo(place);
            return View(place);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdatePlaceDto command )
        {
            if ( !ModelState.IsValid )
            {

                command = ( UpdatePlaceDto ) await FillPlaceInfo(command);
                command.CurrentImages = await _placeImageRepo.GetAllAsync<PlaceImageDto>(
                    filter: u => u.PlaceId == command.Id ,
                    select: entity => _mapper.Map<PlaceImageDto>(entity));

                return View(command);
            }

            var files = HttpContext.Request.Form.Files;

            if ( files.Any() )
            {
                foreach ( var file in files )
                {
                    if ( file.Length > SD.ImageSizeLimit )
                    {
                        command = ( UpdatePlaceDto ) await FillPlaceInfo(command);
                        TempData[SD.Error] = $"سایز عکس وارد شده باید کمتر از {SD.ImageSizeLimitDisplay} باشد";
                        return View(command);
                    }
                }
            }

            await _placeService.UpdateAsync(command , files);
            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";

            return RedirectToAction(nameof(Index) , _filters);
        }


        public async Task<JsonResult> Remove ( Guid id )
        {
            var entity = await _placeRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source.Include(u => u.Images)
                    .Include(u => u.ActivityPlan));

            if ( entity == null )
                return Json(new { Success = false });

            if ( entity.ActivityPlan != null )
                entity.ActivityPlan.RemoveFile();


            entity.Images.ToList().ForEach(img => img.DeleteImage());
            _placeRepo.Remove(entity);
            await _placeRepo.SaveAsync();

            return Json(new { Success = true });
        }
        public async Task<JsonResult> RemoveImage ( long id )
        {
            var img = await _placeImageRepo.FindAsync(id);

            if ( img == null )
                return Json(new { Success = false });

            img.DeleteImage();
            _placeImageRepo.Remove(img);
            await _placeImageRepo.SaveAsync();

            return Json(new { Success = true });
        }


        public async Task<IActionResult> ActivityPlan ( Guid id )
        {
            var plan = await _activityPlanRepo.FirstOrDefaultAsync(u => u.Place.Id == id);

            var vm = new ActivityPlanVM { PlaceId = id , ActivityPlan = _mapper.Map<ActivityPlanDto>(plan) };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> ActivityPlan ( ActivityPlanVM command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            var files = HttpContext.Request.Form.Files;
            if ( files.Any() )
            {
                var entity = await _activityPlanRepo.FirstOrDefaultAsync(u => u.PlaceId == command.PlaceId);
                if ( entity != null )
                {
                    entity.RemoveFile();
                    _activityPlanRepo.Remove(entity);
                }

                var plan = new ActivityPlan(files[0].FileName , new Document(files[0].FileName , files[0].ReadBytes()) , command.PlaceId);
                plan.SaveFile();
                _activityPlanRepo.Add(plan);
                await _activityPlanRepo.SaveAsync();

                TempData[SD.Info] = "برنامه فعالیت جدید ذخیره شد";
            }

            return Redirect(Request.GetTypedHeaders().Referer.ToString());
        }

        [HttpDelete]
        public async Task<JsonResult> RemoveActivityPlan ( long id )
        {
            var entity = await _activityPlanRepo.FirstOrDefaultAsync(u => u.Id == id);
            if ( entity == null )
                return Json(new { Success = false });


            entity.RemoveFile();
            _activityPlanRepo.Remove(entity);
            await _activityPlanRepo.SaveAsync();
            return Json(new { Success = true });
        }
        public async Task<FileResult> DownloadActivityPlan ( long id )
        {
            var entity = await _activityPlanRepo.FindAsync(id);
            if ( entity == null )
            {
                return null;
            }

            string filePath = _hostEnv.WebRootPath + StaticEntitiesDetails.ActivityPlanPath + entity.File.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes , "application/force-download" , entity.Name);

        }


        public IActionResult Download ( String file )
        {
            string filePath = _hostEnv.WebRootPath + StaticEntitiesDetails.PlaceImagePath + file;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes , "application/force-download" , $"{new Random().Next(100 , int.MaxValue)}{Path.GetExtension(file)}");
        }



        private async Task<CreatePlaceDto> FillPlaceInfo ( CreatePlaceDto command )
        {

            command.Places = await _placeRepo.GetAllAsync<SelectListItem>(
                filter: entity => entity.ParentPlaceId == null ,
                select: entity => new SelectListItem { Text = entity.Title , Value = entity.Id.ToString() });
            command.Types = await _placeTypeRepo.GetAllAsync<SelectListItem>(select: entity => new SelectListItem { Text = entity.Title , Value = entity.Id.ToString() });
            command.Sports = await _sportRepo.GetAllAsync<SelectListItem>(select: entity => new SelectListItem { Text = entity.Name , Value = entity.Id.ToString() });

            return command;
        }
    }
}
