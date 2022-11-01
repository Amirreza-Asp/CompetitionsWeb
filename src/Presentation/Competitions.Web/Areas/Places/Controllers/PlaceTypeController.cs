using Competitions.Common;
using Competitions.Domain.Dtos.Places.PlaceTypes;
using Competitions.Domain.Entities.Places;
using Competitions.Web.Areas.Places.Models.PlaceTypes;
using Competitions.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Competitions.Web.Areas.Places.Controllers
{

	[Area("Places")]
	[Authorize($"{SD.Publisher},{SD.Admin}")]
	public class PlaceTypeController : Controller
	{
		private readonly IRepository<PlaceType> _repo;

		private static Pagenation _pagenation = new Pagenation(0 , 10 , 0);

		public PlaceTypeController ( IRepository<PlaceType> repo )
		{
			_repo = repo;
		}


		public async Task<IActionResult> Index ( Pagenation pagenation )
		{
			_pagenation = pagenation;

			var vm = new GetAllPlaceTypesVM
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
				TempData[SD.Error] = "نوع مکان انتخاب شده وجود ندارد";
				return RedirectToAction(nameof(Index) , _pagenation);
			}

			return View(entity);
		}


		public IActionResult Create () => View();
		[HttpPost]
		public async Task<IActionResult> Create ( CreatePlaceTypeDto command )
		{
			if ( !ModelState.IsValid )
				return View(command);

			var entity = new PlaceType(command.Title , command.Description);
			_repo.Add(entity);
			await _repo.SaveAsync();

			TempData[SD.Success] = "نوع مکان با موفقیت ذخیره شد";
			return RedirectToAction(nameof(Index) , _pagenation);
		}


		public async Task<IActionResult> Update ( long id )
		{
			var entity = await _repo.FindAsync(id);

			if ( entity == null )
			{
				TempData[SD.Error] = "نوع مکان انتخاب شده وجود ندارد";
				return RedirectToAction(nameof(Index) , _pagenation);
			}

			return View(UpdatePlaceTypeDto.Create(entity));
		}
		[HttpPost]
		public async Task<IActionResult> Update ( UpdatePlaceTypeDto command )
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
