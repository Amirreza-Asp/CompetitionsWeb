using Competitions.Domain.Entities.Managment;
using Competitions.Domain.Entities.Notifications;
using Competitions.Domain.Entities.Sports;
using Competitions.Web.Models.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Competitions.Web.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Match> _matchRepo;
        private readonly IRepository<Notification> _notifRepo;
        private readonly IRepository<Sport> _sportRepo;

        public HomeController(ILogger<HomeController> logger, IRepository<Match> matchRepo, IRepository<Notification> notifRepo, IRepository<Sport> sportRepo)
        {
            _logger = logger;
            _matchRepo = matchRepo;
            _notifRepo = notifRepo;
            _sportRepo = sportRepo;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var data = new MatchHomeVM
                {
                    Matches = await _matchRepo.GetAllAsync(
                        include: source => source.Include(u => u.Sport),
                        orderBy: source => source.OrderByDescending(u => u.PutOn.From),
                        take: 2),
                    Notifications = await _notifRepo.GetAllAsync(
                        orderBy: source => source.OrderByDescending(u => u.CreateDate),
                        include: source => source.Include(u => u.Images),
                        take: 3)
                };
                return View(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            TempData["LoginHome"] = true;
            return View();
        }

        public async Task<IActionResult> Prog()
        {
            var sports = await _sportRepo.GetAllAsync(
                select: entity => new ProgVM { Id = entity.Id, Name = entity.Name, Image = entity.Image.Name });

            return View(sports);
        }

    }
}