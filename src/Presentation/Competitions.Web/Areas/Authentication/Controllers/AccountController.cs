using Competitions.Application.Authentication.Interfaces;
using Competitions.Application.Managment.Interfaces;
using Competitions.Common;
using Competitions.Common.Helpers;
using Competitions.Domain.Dtos.Authentication.User;
using Competitions.Domain.Entities.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Competitions.Web.Areas.Authentication.Controllers
{
    [Area("Authentication")]
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IRepository<User> _userRepo;
        private readonly ISmsService _smsService;
        private readonly IUserAPI _userAPI;

        public AccountController(IAuthService authService, IRepository<User> userRpeo, ISmsService smsService, IUserAPI userAPI)
        {
            _authService = authService;
            _userRepo = userRpeo;
            _smsService = smsService;
            _userAPI = userAPI;
        }


        public IActionResult Register() => View(new RegisterDto());
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto command)
        {
            if (!ModelState.IsValid)
                return View(command);

            if (_userRepo.GetCount(u => u.NationalCode.Value == command.NationalCode) != 0)
            {
                TempData[SD.Warning] = "کاربر قبلا در سیستم ثبت شده است";
                return View(command);
            }

            var user = await _userAPI.GetUserAsync(command.NationalCode);
            if (user == null)
            {
                TempData[SD.Error] = "کد ملی وارد شده در سیستم وجود ندارد";
                return View(command);
            }

            if (command.StudentNumber != user.student_number.ToString() && user.type.ToLower() == "student")
            {
                TempData[SD.Error] = "شماره دانشجویی وارد شده با کد ملی مطابقت ندارد";
                return View(command);
            }

            var res = await _authService.RegisterAsync(command);
            if (res.Success)
            {
                var fullName = await _userRepo.FirstOrDefaultSelectAsync<String>(filter: u => u.NationalCode.Value == command.NationalCode,
                    select: s => String.Concat(s.Name, " ", s.Family));

                TempData[SD.Success] = $"{fullName} خوش آمدید";
                return Redirect("/Home");
            }

            TempData[SD.Error] = res.Message;
            return View(command);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto command)
        {
            if (!ModelState.IsValid)
                return View(command);

            var res = await _authService.LoginAsync(command);
            if (res.Success)
            {
                var user = await _userRepo.FirstOrDefaultAsync(
                    filter: u => u.UserName == command.UserName);

                TempData[SD.Success] = $"{String.Concat(user.Name, ' ', user.Family)} خوش امدید";

                return Redirect("/Home/Index");
            }

            TempData[SD.Error] = res.Message;
            return View(command);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto command)
        {
            if (!ModelState.IsValid)
                return View(command);

            var nationalCode = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (nationalCode == null)
            {
                TempData[SD.Error] = "عملیات با شکست مواجه شد";
                return View(command);
            }

            await _authService.ChangePasswordAsync(command);
            TempData[SD.Success] = "رمز عبور با موفقیت تغییر یافت";
            return RedirectToAction(nameof(Logout));
        }


        public IActionResult ForgetPassword() => View();
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto command)
        {
            if (!ModelState.IsValid)
                return View(command);

            var user = await _userRepo.FirstOrDefaultAsync(u => u.NationalCode.Value == command.NationalCode);

            if (user == null)
            {
                TempData[SD.Error] = "کد ملی وارد شده در سیستم وجود ندارد";
                return View(command);
            }

            Random rnd = new Random();
            command.SecretCode = rnd.Next(10000, 99999);
            HttpContext.Session.Set<ForgetPasswordDto>(SD.ForgetPassword, command);


            await _smsService.SendAsync($"کد فراموشی : {command.SecretCode}", user.PhoneNumber.Value);
            TempData[SD.Info] = "کد از طریق پیامک ارسال شد";
            return RedirectToAction(nameof(ReciveCode));
        }

        public IActionResult ReciveCode() => View();
        [HttpPost]
        public async Task<IActionResult> ReciveCode(ReciveCodeDto command)
        {
            var dto = HttpContext.Session.Get<ForgetPasswordDto>(SD.ForgetPassword);
            if (command.Code != dto.SecretCode.ToString())
            {
                TempData[SD.Error] = "کد وارد شده اشتباه است";
                return View(command);
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, dto.NationalCode));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            HttpContext.Session.Remove(SD.ForgetPassword);

            return RedirectToAction(nameof(ChangePassword));
        }

        public async Task<IActionResult> KhedmatLogin(String nationalCode)
        {
            await _authService.KhemdatLoginAsync(nationalCode);
            return Redirect("/Home/Index");
        }

        private bool ValidUrl(String url)
        {
            return url.Contains("https://khedmat.razi.ac.ir");
        }
    }
}
