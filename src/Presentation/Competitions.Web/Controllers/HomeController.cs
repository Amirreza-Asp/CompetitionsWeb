﻿using Competitions.Domain.Entities.Managment;
using Competitions.Domain.Entities.Notifications;
using Competitions.Domain.Entities.Sports;
using Competitions.Web.Models;
using Competitions.Web.Models.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Competitions.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Match> _matchRepo;
        private readonly IRepository<Notification> _notifRepo;
        private readonly IRepository<Sport> _sportRepo;

        public HomeController ( ILogger<HomeController> logger , IRepository<Match> matchRepo , IRepository<Notification> notifRepo , IRepository<Sport> sportRepo )
        {
            _logger = logger;
            _matchRepo = matchRepo;
            _notifRepo = notifRepo;
            _sportRepo = sportRepo;
        }

        public async Task<IActionResult> Index ()
        {
            var vm = new MatchHomeVM
            {
                Matches = await _matchRepo.GetAllAsync(
                    include: source => source.Include(u => u.Sport) ,
                    orderBy: source => source.OrderByDescending(u => u.PutOn.From) ,
                    take: 2) ,
                Notifications = await _notifRepo.GetAllAsync(
                    orderBy: source => source.OrderByDescending(u => u.CreateDate) ,
                    take: 3)
            };

            return View(vm);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Prog ()
        {
            var sports = await _sportRepo.GetAllAsync(
                select: entity => new ProgVM { Id = entity.Id , Name = entity.Name , Image = entity.Image.Name });

            return View(sports);
        }

        public IActionResult Privacy ()
        {
            return View();
        }

        [ResponseCache(Duration = 0 , Location = ResponseCacheLocation.None , NoStore = true)]
        public IActionResult Error ()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}