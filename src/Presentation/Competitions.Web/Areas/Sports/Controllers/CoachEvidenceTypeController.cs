using AutoMapper;
using Competitions.Common;
using Competitions.Domain.Dtos.Sports.CoachEvidenceTypes;
using Competitions.Domain.Entities.Sports;
using Competitions.Web.Areas.Sports.Models.CoachEvidenceTypes;
using Competitions.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Competitions.Web.Areas.Sports.Controllers
{
	[Area("Sports")]
	[Authorize(Roles = $"{SD.Publisher},{SD.Admin}")]
	public class CoachEvidenceTypeController : Controller
	{
		private readonly IRepository<CoachEvidenceType> _repo;
		private readonly IMapper _mapper;

		private static Pagenation _pagenation = new Pagenation();

		public CoachEvidenceTypeController ( IRepository<CoachEvidenceType> repo , IMapper mapper )
		{
			_repo = repo;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index ( Pagenation pagenation )
		{
			_pagenation = pagenation;
			var vm = new GetAllCETVM
			{
				Entities = await _repo.GetAllAsync(skip: pagenation.Skip , take: pagenation.Take) ,
				Pagenation = pagenation
			};

			return View(vm);
		}


		public async Task<IActionResult> Details ( long id )
		{
			var entity = await _repo.FindAsync(id);
			if ( entity == null )
			{
				TempData[SD.Error] = "نوع مدرک مورد نظر وجود ندارد";
				return RedirectToAction(nameof(Index) , _pagenation);
			}

			return View(entity);
		}


		public IActionResult Create () => View();
		[HttpPost]
		public async Task<IActionResult> Create ( CreateCoachEvidenceTypeDto command )
		{
			if ( !ModelState.IsValid )
				return View(command);

			var entity = new CoachEvidenceType(command.Title , command.Description);
			_repo.Add(entity);
			await _repo.SaveAsync();

			TempData[SD.Success] = "نوع مدرک مربیگری با موفقیت ذخیره شد";
			return RedirectToAction(nameof(Index) , _pagenation);
		}


		public async Task<IActionResult> Update ( long id )
		{
			var entity = await _repo.FindAsync(id);
			if ( entity == null )
			{
				TempData[SD.Error] = "نوع مدرک مورد نظر وجود ندارد";
				return RedirectToAction(nameof(Index) , _pagenation);
			}

			return View(UpdateCoachEvidenceTypeDto.Create(entity));
		}
		[HttpPost]
		public async Task<IActionResult> Update ( UpdateCoachEvidenceTypeDto command )
		{
			if ( !ModelState.IsValid )
				return View(command);

			CoachEvidenceType entity = await _repo.FindAsync(command.Id);
			entity.WithDescription(command.Description)
				.WithTitle(command.Title);

			_repo.Update(entity);
			await _repo.SaveAsync();

			TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
			return RedirectToAction(nameof(Index) , _pagenation);
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
