using AutoMapper;
using Competitions.Application.Authentication.Interfaces;
using Competitions.Common;
using Competitions.Domain.Dtos.Authentication.User;
using Competitions.Domain.Entities.Authentication;
using Competitions.Domain.Entities.Authentication.Spec;
using Competitions.Web.Areas.Authentication.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Competitions.Web.Areas.Authentication.Controllers
{
    [Area("Authentication")]
    [Authorize(Roles = $"{SD.Admin}")]
    public class UserController : Controller
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Role> _roleRepo;
        private readonly IUserAPI _userAPI;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPositionAPI _positionAPI;

        private static UserFilter _filters = new UserFilter();

        public UserController(IRepository<User> userRepo, IRepository<Role> roleRepo, IMapper mapper, IUserAPI userAPI, IPasswordHasher passwordHasher, IPositionAPI positionAPI)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _mapper = mapper;
            _userAPI = userAPI;
            _passwordHasher = passwordHasher;
            _positionAPI = positionAPI;
        }

        public async Task<IActionResult> Index(UserFilter filter)
        {
            _filters = filter;
            var spec = new GetFilteredUsersSpec(filter.Name, filter.Family, filter.RoleId, filter.NationalCode, filter.Skip, filter.Take);
            filter.Total = _userRepo.GetCount();
            filter.Roles = await _roleRepo.GetAllAsync<SelectListItem>(
                filter: u => u.Title != SD.User,
                select: u => new SelectListItem { Text = u.Display, Value = u.Id.ToString() });

            var vm = new GetAllUsersVM
            {
                Users = _userRepo.GetAll<UserDetails>(spec, entity => _mapper.Map<UserDetails>(entity)),
                Filters = filter
            };

            return View(vm);
        }


        public async Task<IActionResult> Create()
        {
            var position = await _positionAPI.GetPositionsAsync();
            var command = new CreateUserDto
            {
                Roles = await _roleRepo.GetAllAsync<SelectListItem>(select: u => new SelectListItem { Text = u.Display, Value = u.Id.ToString() }),
                Positions = position.Select(u => new SelectListItem { Text = u.Title, Value = u.Title })
            };
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto command)
        {
            if (!ModelState.IsValid)
            {
                var position = await _positionAPI.GetPositionsAsync();
                command.Roles = await _roleRepo.GetAllAsync<SelectListItem>(select: u => new SelectListItem { Text = u.Display, Value = u.Id.ToString() });
                command.Positions = position.Select(u => new SelectListItem { Text = u.Title, Value = u.Title });
                return View(command);
            }

            var user = await _userAPI.GetUserAsync(command.NationalCode);
            if (user == null)
            {
                var position = await _positionAPI.GetPositionsAsync();
                command.Roles = await _roleRepo.GetAllAsync<SelectListItem>(select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });
                command.Positions = position.Select(u => new SelectListItem { Text = u.Title, Value = u.Title });
                TempData[SD.Error] = "هیچ کاربری برای کد ملی وارد شده در سیستم ثبت نشده";
                return View(command);
            }

            var userEntity = new User(user.Name, user.Lastname, user.Mobile, user.Idmelli, user.Idmelli, _passwordHasher.HashPassword(user.Idmelli),
                command.RoleId, user.StudentNumber.ToString(), user.Trend, false);

            _userRepo.Add(userEntity);
            await _userRepo.SaveAsync();

            TempData[SD.Success] = "کاربر با موفقیت ذخیره شد";
            return RedirectToAction(nameof(Index), _filters);
        }


        public async Task<IActionResult> Update(Guid id)
        {
            var entity = await _userRepo.FirstOrDefaultAsync(u => u.Id == id, source => source.Include(u => u.Role));
            if (entity == null)
            {
                TempData[SD.Error] = "کاربر انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index), _filters);
            }

            if (entity.Role.Title.ToLower() == "admin")
            {
                TempData[SD.Error] = "کاربر با دسترسی ادمین را نمیتوان ویرایش کرد";
                return RedirectToAction(nameof(Index), _filters);
            }

            var command = _mapper.Map<UpdateUserDto>(entity);
            var position = await _positionAPI.GetPositionsAsync();
            command.Roles = await _roleRepo.GetAllAsync<SelectListItem>(select: u => new SelectListItem { Text = u.Display, Value = u.Id.ToString() });
            command.Positions = position.Select(u => new SelectListItem { Text = u.Title, Value = u.Title });

            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserDto command)
        {
            if (!ModelState.IsValid)
            {
                var position = await _positionAPI.GetPositionsAsync();
                command.Roles = await _roleRepo.GetAllAsync<SelectListItem>(select: u => new SelectListItem { Text = u.Display, Value = u.Id.ToString() });
                command.Positions = position.Select(u => new SelectListItem { Text = u.Title, Value = u.Title });
                return View(command);
            }

            User entity = await _userRepo.FindAsync(command.Id);
            entity.WithRole(command.RoleId);
            _userRepo.Update(entity);
            await _userRepo.SaveAsync();

            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index), _filters);
        }


        public async Task<IActionResult> Details(Guid id)
        {
            var entity = await _userRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id,
                include: source => source.Include(u => u.Role));

            if (entity == null)
            {
                TempData[SD.Error] = "کاربر انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index));
            }

            return View(entity);
        }


        public async Task<IActionResult> Remove(Guid id)
        {
            User entity = _userRepo.FirstOrDefault(
                filter: u => u.Id == id,
                include: source => source.Include(u => u.Role));

            if (entity == null || entity.Role.Title.ToLower() == "admin")
                return Json(new { Success = false });

            entity.Delete();
            _userRepo.Update(entity);
            await _userRepo.SaveAsync();

            return Json(new { Success = true });
        }



        public async Task<JsonResult> GetUserInfo(String id)
        {
            var user = await _userAPI.GetUserAsync(id);
            if (user == null)
                return Json(new { Exists = false });

            return Json(new
            {
                Exists = true,
                Info = new
                {
                    FullName = $"{user.Name} {user.Lastname}",
                    NationalCode = user.Idmelli,
                    Email = user.Email,
                    PhoneNumber = user.Mobile
                }
            });
        }
        public async Task<JsonResult> GetPositions(String id)
        {
            var users = await _positionAPI.GetUsersAsync(id);
            if (users != null)
            {
                var data = users.Select(u => new SelectListItem { Text = u.Name, Value = u.NationalCode });
                return Json(new { Exists = true, Data = data });
            }
            return Json(new { Success = false });
        }
    }
}
