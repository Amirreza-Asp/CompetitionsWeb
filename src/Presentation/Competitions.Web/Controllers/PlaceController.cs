using Competitions.Common;
using Competitions.Domain.Entities;
using Competitions.Domain.Entities.Places;
using Competitions.Web.Models.Places;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Competitions.Web.Controllers
{
    public class PlaceController : Controller
    {
        private readonly IRepository<Place> _placeRepo;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IRepository<ActivityPlan> _actRepo;

        public PlaceController ( IRepository<Place> placeRepo , IWebHostEnvironment hostEnv , IRepository<ActivityPlan> actRepo )
        {
            _placeRepo = placeRepo;
            _hostEnv = hostEnv;
            _actRepo = actRepo;
        }

        public async Task<IActionResult> Index ()
        {
            var places = await _placeRepo.GetAllAsync(
                filter: u => u.ParentPlaceId == null ,
                include: source => source.Include(u => u.Supervisor).Include(u => u.Images) ,
                select: u => new PlaceListDto
                {
                    Id = u.Id ,
                    Address = u.Address ,
                    Image = u.Images.FirstOrDefault() == null ? "" : u.Images.First().Image.Name ,
                    Name = u.Title ,
                    SupervisorName = u.Supervisor.Name ,
                    SupervisorPhone = u.Supervisor.PhoneNumber
                });

            return View(places);
        }

        public async Task<IActionResult> Details ( Guid id )
        {
            var entity = await _placeRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source.Include(u => u.Images)
                    .Include(u => u.Supervisor)
                    .Include(u => u.PlaceType)
                    .Include(u => u.Sports)
                        .ThenInclude(u => u.Sport)
                    .Include(u => u.SubPlaces)
                        .ThenInclude(u => u.Images)
                     .Include(u => u.SubPlaces)
                        .ThenInclude(u => u.Supervisor));

            if ( entity == null )
            {
                TempData[SD.Warning] = "مکان انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index));
            }

            return View(entity);
        }

        public async Task<IActionResult> DownloadActivityPlan ( Guid id )
        {
            var entity = await _actRepo.FirstOrDefaultAsync(u => u.PlaceId == id);
            if ( entity == null )
            {
                TempData[SD.Warning] = "برنامه فعالیت برای این مکان تعریف نشده";
                return Redirect(Request.GetTypedHeaders().Referer.ToString());
            }

            string filePath = _hostEnv.WebRootPath + StaticEntitiesDetails.ActivityPlanPath + entity.File.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes , "application/force-download" , entity.Name);

        }
    }
}
