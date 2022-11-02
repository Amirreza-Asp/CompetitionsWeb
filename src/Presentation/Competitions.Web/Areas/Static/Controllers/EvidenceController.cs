using Competitions.Common;
using Competitions.Domain.Dtos.Static.Evidences;
using Competitions.Domain.Entities.Static;
using Competitions.Web.Models;
using Competitions.Web.Models.Evidences;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Competitions.Web.Areas.Static.Controllers
{
    [Area("Static")]
    [Authorize(Roles = $"{SD.Publisher},{SD.Admin}")]
    public class EvidenceController : Controller
    {
        private readonly IRepository<Evidence> _repo;

        private static Pagenation _pagenation = new Pagenation(0 , 10 , 0);

        public EvidenceController ( IRepository<Evidence> repo )
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index ( Pagenation pagenation )
        {
            _pagenation = pagenation;

            var vm = new GetAllEvidenceVM
            {
                Entities = await _repo.GetAllAsync(skip: pagenation.Skip , take: pagenation.Take) ,
                Pagenation = new Pagenation(pagenation.Skip , pagenation.Take , _repo.GetCount())
            };

            return View(vm);
        }


        public async Task<IActionResult> Details ( long id )
        {
            var entity = await _repo.FindAsync(id);

            if ( entity == null )
            {
                TempData[SD.Error] = "مدرک انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _pagenation);
            }

            return View(entity);
        }


        public IActionResult Create () => View();
        [HttpPost]
        public async Task<IActionResult> Create ( CreateEvidenceDto command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            var entity = new Evidence(command.Title , command.Description);
            _repo.Add(entity);
            await _repo.SaveAsync();

            TempData[SD.Success] = "مدرک با موفقیت ذخیره شد";
            return RedirectToAction(nameof(Index) , _pagenation);
        }


        public async Task<IActionResult> Update ( long id )
        {
            var entity = await _repo.FindAsync(id);

            if ( entity == null )
            {
                TempData[SD.Error] = "مدرک انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _pagenation);
            }

            return View(UpdateEvidenceDto.Create(entity));
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateEvidenceDto command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            var entity = await _repo.FindAsync(command.Id);
            entity.WithTitle(command.Title)
                .WithDescription(command.Description);

            _repo.Update(entity);
            await _repo.SaveAsync();

            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index) , _pagenation);
        }


        public async Task<JsonResult> Remove ( long id )
        {
            var entity = await _repo.FindAsync(id);
            if ( entity == null )
                return Json(new { Success = false });

            _repo.Remove(entity);
            await _repo.SaveAsync();

            return Json(new { Success = true });
        }
    }
}
