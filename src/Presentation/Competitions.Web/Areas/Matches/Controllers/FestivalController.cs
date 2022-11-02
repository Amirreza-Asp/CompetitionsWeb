using AutoMapper;
using Competitions.Common;
using Competitions.Common.Helpers;
using Competitions.Domain.Dtos.Matches.Festivals;
using Competitions.Domain.Entities.Managment;
using Competitions.Domain.Entities.Managment.Spec;
using Competitions.Domain.Entities.Managment.ValueObjects;
using Competitions.SharedKernel.ValueObjects;
using Competitions.Web.Areas.Managment.Models.Festivals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Competitions.Web.Areas.Managment.Controllers
{
    [Area("Matches")]
    [Authorize(Roles = $"{SD.Publisher},{SD.Admin}")]
    public class FestivalController : Controller
    {
        private readonly IRepository<Festival> _festivalRepo;
        private readonly IMapper _mapper;

        private static FestivalFilter _filters = new FestivalFilter();

        public FestivalController ( IRepository<Festival> festivalRepo , IMapper mapper )
        {
            _festivalRepo = festivalRepo;
            _mapper = mapper;
        }

        public IActionResult Index ( FestivalFilter filters )
        {
            _filters = filters;

            var spec = new GetFilteredFestivalsSpec(filters.Skip , filters.Take);

            filters.Total = _festivalRepo.GetCount(spec);
            var vm = new GetAllFestivalsVM
            {
                Entities = _festivalRepo.GetAll(spec) ,
                Filters = filters
            };

            return View(vm);
        }


        public async Task<IActionResult> Details ( Guid id )
        {
            var entity = await _festivalRepo.FindAsync(id);
            if ( entity == null )
            {
                TempData[SD.Error] = "جشنواره انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            return View(entity);
        }

        public IActionResult Create () => View(new CreateFestivalDto());
        [HttpPost]
        public async Task<IActionResult> Create ( CreateFestivalDto command )
        {
            command.Start = command.Start.ToMiladi();
            command.End = command.End.ToMiladi();

            if ( !ModelState.IsValid )
                return View(command);

            if ( command.Start.Date > command.End.Date )
            {
                TempData[SD.Error] = "زمان شروع نمیتواند از پایان بیشتر باشد";
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;

            if ( files.Count() == 0 )
            {
                TempData[SD.Error] = "عکس جشنواره را وارد کنید";
                return View(command);
            }

            if ( files[0].ReadBytes().Length > SD.ImageSizeLimit )
            {
                TempData[SD.Error] = $"سایز عکس وارد شده باید کمتر از {SD.ImageSizeLimitDisplay} باشد";
                return View(command);
            }

            var entity = new Festival(command.Title , new DateTimeRange(command.Start , command.End) ,
                command.Description , new Document(files[0].FileName , files[0].ReadBytes()));

            entity.SaveImage();
            _festivalRepo.Add(entity);
            await _festivalRepo.SaveAsync();

            TempData[SD.Success] = "جشنواره با موفقیت ذخیره شد";
            return RedirectToAction(nameof(Index) , _filters);
        }


        public async Task<IActionResult> Update ( Guid id )
        {
            var entity = await _festivalRepo.FindAsync(id);
            if ( entity == null )
            {
                TempData[SD.Error] = "جشنواره انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            var dto = _mapper.Map<UpdateFestivalDto>(entity);
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateFestivalDto command )
        {
            command.Start = command.Start.ToMiladi();
            command.End = command.End.ToMiladi();

            if ( !ModelState.IsValid )
                return View(command);

            if ( command.Start.Date > command.End.Date )
            {
                TempData[SD.Error] = "زمان شروع نمیتواند از پایان بیشتر باشد";
                return View(command);
            }

            Festival entity = await _festivalRepo.FindAsync(command.Id);
            var files = HttpContext.Request.Form.Files;

            if ( files.Any() )
            {
                if ( files[0].ReadBytes().Length > SD.ImageSizeLimit )
                {
                    TempData[SD.Error] = $"سایز عکس وارد شده باید کمتر از {SD.ImageSizeLimitDisplay} باشد";
                    return View(command);
                }


                entity.DeleteImage();
                entity.WithImage(new Document(files[0].FileName , files[0].ReadBytes()));
                entity.SaveImage();
            }

            entity.WithTitle(command.Title)
                .WithDescription(command.Description)
                .WithDuration(new DateTimeRange(command.Start , command.End));

            _festivalRepo.Update(entity);
            await _festivalRepo.SaveAsync();

            TempData[SD.Info] = "ویرایش جشنواره با موفقیت انجام شد";
            return RedirectToAction(nameof(Index) , _filters);
        }


        [HttpDelete]
        public async Task<JsonResult> Remove ( Guid id )
        {
            var entity = await _festivalRepo.FindAsync(id);
            if ( entity == null )
                return Json(new { Success = false });

            entity.DeleteImage();
            _festivalRepo.Remove(entity);
            await _festivalRepo.SaveAsync();

            return Json(new { Success = true });
        }

    }
}
