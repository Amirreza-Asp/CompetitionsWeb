using Competitions.Common;
using Competitions.Domain.Entities.Notifications;
using Competitions.Web.Models;
using Competitions.Web.Models.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Competitions.Web.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly IRepository<Notification> _notifRepo;
        public NotificationController ( IRepository<Notification> notifRepo )
        {
            _notifRepo = notifRepo;
        }

        public async Task<IActionResult> Index ( Pagenation pagenation )
        {
            pagenation.Total = _notifRepo.GetCount();
            var vm = new NotifDataVM
            {
                Notifications = await _notifRepo.GetAllAsync(
                    orderBy: u => u.OrderByDescending(u => u.CreateDate) ,
                    include: source => source.Include(u => u.Images) ,
                    take: pagenation.Take ,
                    skip: pagenation.Skip) ,
                Pagenation = pagenation
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
                return RedirectToAction(nameof(Index));
            }

            return View(notif);
        }

    }
}
