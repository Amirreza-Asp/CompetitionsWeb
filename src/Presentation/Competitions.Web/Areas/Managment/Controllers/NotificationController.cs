using AutoMapper;
using Competitions.Common;
using Competitions.Common.Helpers;
using Competitions.Domain.Dtos.Managment.Notifications;
using Competitions.Domain.Entities.Managment;
using Competitions.Domain.Entities.Managment.Spec;
using Competitions.SharedKernel.ValueObjects;
using Competitions.Web.Areas.Managment.Models.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Competitions.Web.Areas.Managment.Controllers
{
    [Area("Managment")]
    [Authorize($"{SD.Publisher},{SD.Admin}")]
    public class NotificationController : Controller
    {
        private readonly IRepository<Notification> _notifRepo;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IMapper _mapper;

        private static NotificationFilter _filters = new NotificationFilter();

        public NotificationController ( IWebHostEnvironment hostEnv , IRepository<Notification> notifRepo , IMapper mapper )
        {
            _hostEnv = hostEnv;
            _notifRepo = notifRepo;
            _mapper = mapper;
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
            var notif = await _notifRepo.FindAsync(id);
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

            if ( files.Any() && files[0].ReadBytes().Length > SD.ImageSizeLimit )
            {
                TempData[SD.Error] = $"سایز عکس وارد شده باید کمتر از {SD.ImageSizeLimitDisplay} باشد";
                return View(command);
            }

            var notif = new Notification(command.Title , command.Description , new Document(files[0].FileName , files[0].ReadBytes()));
            notif.SaveImage();
            _notifRepo.Add(notif);
            await _notifRepo.SaveAsync();

            TempData[SD.Success] = "اطلاعیه با موفقیت ثبت شد";
            return RedirectToAction(nameof(Index) , _filters);
        }


        public async Task<IActionResult> Update ( long id )
        {
            var notif = await _notifRepo.FindAsync(id);

            if ( notif == null )
            {
                TempData[SD.Error] = "اطلاعیه انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            var dto = _mapper.Map<UpdateNotificationDto>(notif);
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateNotificationDto command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            var files = HttpContext.Request.Form.Files;
            Notification notif = await _notifRepo.FindAsync(command.Id);

            if ( files.Any() && files[0].ReadBytes().Length > SD.ImageSizeLimit )
            {
                TempData[SD.Error] = $"سایز عکس وارد شده باید کمتر از {SD.ImageSizeLimitDisplay} باشد";
                return View(command);
            }

            if ( files.Any() )
            {
                notif.DeleteImage();

                notif.WithImage(new Document(files[0].FileName , files[0].ReadBytes()));
                notif.SaveImage();
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
            var notif = await _notifRepo.FindAsync(id);
            if ( notif == null )
                return Json(new { Success = false });

            notif.DeleteImage();
            _notifRepo.Remove(notif);
            await _notifRepo.SaveAsync();

            return Json(new { Success = true });
        }
    }
}
