using Competitions.Domain.Entities.Managment;
using Competitions.Web.Models;
using Competitions.Web.Models.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace Competitions.Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IRepository<Notification> _notifRepo;
        private static Pagenation _pagenation = new Pagenation(0 , 10 , 0);

        public NotificationController ( IRepository<Notification> notifRepo )
        {
            _notifRepo = notifRepo;
        }

        public async Task<IActionResult> Index ( Pagenation pagenation )
        {
            _pagenation = pagenation;

            var vm = new NotifDataVM
            {
                Notifications = await _notifRepo.GetAllAsync(
                    orderBy: u => u.OrderByDescending(u => u.CreateDate) ,
                    take: _pagenation.Take ,
                    skip: _pagenation.Skip) ,
                Pagenation = new Pagenation(_pagenation.Skip , _pagenation.Take , _notifRepo.GetCount())
            };

            return View(vm);
        }

    }
}
