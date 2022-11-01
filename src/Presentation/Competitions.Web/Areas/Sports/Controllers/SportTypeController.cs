using AutoMapper;
using Competitions.Common;
using Competitions.Domain.Dtos.Sports.SportTypes;
using Competitions.Domain.Entities.Sports;
using Competitions.Web.Areas.Sports.Models.SportTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Competitions.Web.Areas.Sports.Controllers
{
	[Area("Sports")]
	[Authorize($"{SD.Publisher},{SD.Admin}")]
	public class SportTypeController : Controller
	{
		private readonly IRepository<SportType> _repo;
		private readonly IMapper _mapper;

		private static SportTypeFilter _filters = new SportTypeFilter();

		public SportTypeController ( IRepository<SportType> repo , IMapper mapper )
		{
			_repo = repo;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index ( SportTypeFilter filters )
		{
			_filters = filters;
			var vm = new GetAllSportTypesVM
			{
				Entities = await _repo.GetAllAsync(skip: filters.Skip , take: filters.Take) ,
				Filters = new SportTypeFilter { TotalCount = _repo.GetCount() }
			};

			return View(vm);
		}


		public async Task<IActionResult> Details ( long id )
		{
			var entity = await _repo.FindAsync(id);
			if ( entity == null )
			{
				TempData[SD.Error] = "رشته مورد نظر وجود ندارد";
				return RedirectToAction(nameof(Index) , _filters);
			}

			return View(entity);
		}


		public IActionResult Create () => View();
		[HttpPost]
		public async Task<IActionResult> Create ( CreateSportTypeDto command )
		{
			if ( !ModelState.IsValid )
				return View(command);

			var entity = new SportType(command.Title , command.Description);
			_repo.Add(entity);
			await _repo.SaveAsync();

			return RedirectToAction(nameof(Index) , _filters);
		}


		public async Task<IActionResult> Update ( long id )
		{
			var entity = await _repo.FindAsync(id);
			if ( entity == null )
			{
				TempData[SD.Error] = "نوع رشته مورد نظر وجود ندارد";
				return RedirectToAction(nameof(Index) , _filters);
			}

			return View(UpdateSportTypeDto.Create(entity));
		}
		[HttpPost]
		public async Task<IActionResult> Update ( UpdateSportTypeDto command )
		{
			if ( !ModelState.IsValid )
				return View(command);

			SportType entity = await _repo.FindAsync(command.Id);
			entity.WithDescription(command.Description)
				.WithTitle(command.Title);

			_repo.Update(entity);
			await _repo.SaveAsync();

			TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
			return RedirectToAction(nameof(Index) , _filters);
		}


		[HttpDelete]
		public async Task<JsonResult> Remove ( long id )
		{
			var entity = _repo.Find(id);
			if ( entity == null )
				return Json(new { Success = false });

			_repo.Remove(entity);
			await _repo.SaveAsync();
			return Json(new { Success = true });
		}
	}
}
