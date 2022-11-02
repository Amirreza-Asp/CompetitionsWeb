using AutoMapper;
using Competitions.Common;
using Competitions.Common.Helpers;
using Competitions.Domain.Dtos.Notifications;
using Competitions.Domain.Entities;
using Competitions.Domain.Entities.Notifications;
using Competitions.Domain.Entities.Notifications.Spec;
using Competitions.SharedKernel.ValueObjects;
using Competitions.Web.Areas.Managment.Models.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Competitions.Web.Areas.Notifications.Controllers
{
    [Area("Notifications")]
    [Authorize(Roles = $"{SD.Publisher},{SD.Admin}")]
    public class NotificationController : Controller
    {
        private readonly IRepository<Notification> _notifRepo;
        private readonly IRepository<NotificationImage> _notifImageRepo;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IMapper _mapper;

        private static NotificationFilter _filters = new NotificationFilter();

        public NotificationController ( IWebHostEnvironment hostEnv , IRepository<Notification> notifRepo , IMapper mapper , IRepository<NotificationImage> notifImageRepo )
        {
            _hostEnv = hostEnv;
            _notifRepo = notifRepo;
            _mapper = mapper;
            _notifImageRepo = notifImageRepo;
        }

        public IActionResult Index ( NotificationFilter filters )
        {
            _filters = filters;

            var spec = new GetFilteredNotificationsSpec(filters.Skip , filters.Take);

            var vm = new GetAllNotificationsVM
            {
                Entities = _notifRepo.GetAll(spec) ,
                Filters = new NotificationFilter { Skip = filters.Skip , Take = filters.Take , Total = _notifRepo.GetCount() }
            };

            return View(vm);
        }


        public async Task<IActionResult> Details ( long id )
        {
            var notif = await _notifRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source.Include(u => u.Images));

            if ( notif == null )
            {
                TempData[SD.Error] = "اطلاعیه انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            return View(notif);
        }


        public IActionResult Create () => View();
        [HttpPost]
        public async Task<IActionResult> Create ( CreateNotificationDto command )
        {
            var files = HttpContext.Request.Form.Files;
            if ( !ModelState.IsValid || !files.Any() )
                return View(command);


            foreach ( var file in files )
            {
                if ( file.ReadBytes().Length > SD.ImageSizeLimit )
                {
                    TempData[SD.Error] = $"سایز عکس وارد شده باید کمتر از {SD.ImageSizeLimitDisplay} باشد";
                    return View(command);
                }

            }

            var notif = new Notification(command.Title , command.Description);
            files.ToList().ForEach(file =>
            {
                var img = new NotificationImage(file.FileName , new Document(file.FileName , file.ReadBytes()));
                img.SaveImage();
                notif.AddImage(img);
            });

            _notifRepo.Add(notif);
            await _notifRepo.SaveAsync();

            TempData[SD.Success] = "اطلاعیه با موفقیت ثبت شد";
            return RedirectToAction(nameof(Index) , _filters);
        }


        public async Task<IActionResult> Update ( long id )
        {
            var notif = await _notifRepo.FirstOrDefaultAsync(
               filter: u => u.Id == id ,
               include: source => source.Include(u => u.Images));

            if ( notif == null )
            {
                TempData[SD.Error] = "اطلاعیه انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            var dto = _mapper.Map<UpdateNotificationDto>(notif);
            dto.CurrentImages = await _notifImageRepo.GetAllAsync<NotificaionImageDto>(
                filter: u => u.Notification.Id == id ,
                    select: img => new NotificaionImageDto { Name = img.Image.Name , Id = img.Id });
            //dto.CurrentImages = new List<NotificaionImageDto>();

            //foreach ( var img in notif.Images )
            //{
            //    dto.CurrentImages.Append(new NotificaionImageDto { Id = img.Id , Name = img.Image.Name });
            //}

            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateNotificationDto command )
        {
            if ( !ModelState.IsValid )
            {
                command.CurrentImages = await _notifImageRepo.GetAllAsync<NotificaionImageDto>(
                    filter: u => u.Notification.Id == command.Id ,
                    select: img => new NotificaionImageDto { Name = img.Image.Name , Id = img.Id });

                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            var notif = await _notifRepo.FindAsync(command.Id);


            if ( files != null )
                foreach ( var file in files )
                {
                    if ( file.ReadBytes().Length > SD.ImageSizeLimit )
                    {
                        command.CurrentImages = await _notifImageRepo.GetAllAsync<NotificaionImageDto>(
                            filter: u => u.Notification.Id == command.Id ,
                            select: img => new NotificaionImageDto { Name = img.Image.Name , Id = img.Id });

                        TempData[SD.Error] = $"سایز عکس وارد شده باید کمتر از {SD.ImageSizeLimitDisplay} باشد";
                        return View(command);
                    }

                    var img = new NotificationImage(file.FileName , new Document(file.FileName , file.ReadBytes()));
                    img.SaveImage();
                    notif.AddImage(img);
                }


            notif.WithTitle(command.Title)
                .WithDescription(command.Description);

            _notifRepo.Update(notif);
            await _notifRepo.SaveAsync();

            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index) , _filters);
        }


        [HttpDelete]
        public async Task<JsonResult> Remove ( long id )
        {
            var notif = await _notifRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source.Include(u => u.Images));

            if ( notif == null )
                return Json(new { Success = false });

            if ( notif.Images != null )
                notif.Images.ToList().ForEach(img => img.DeleteImage());

            _notifRepo.Remove(notif);
            await _notifRepo.SaveAsync();

            return Json(new { Success = true });
        }
        [HttpDelete]
        public async Task<JsonResult> RemoveImage ( long id )
        {
            var img = await _notifImageRepo.FindAsync(id);
            if ( img == null )
                return Json(new { Success = false });

            img.DeleteImage();
            _notifImageRepo.Remove(img);
            await _notifImageRepo.SaveAsync();

            return Json(new { Success = true });
        }

        public async Task<FileResult> DownloadImage ( long id )
        {
            var img = await _notifImageRepo.FindAsync(id);
            if ( img == null )
            {
                return null;
            }

            string filePath = _hostEnv.WebRootPath + StaticEntitiesDetails.NotificationPath + img.Image.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes , "application/force-download" , img.Name);

        }


    }
}
