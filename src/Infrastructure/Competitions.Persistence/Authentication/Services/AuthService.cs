using Competitions.Application;
using Competitions.Application.Authentication.Interfaces;
using Competitions.Common;
using Competitions.Domain.Dtos.Authentication.User;
using Competitions.Domain.Entities.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Competitions.Persistence.Authentication.Services
{
    public class AuthService : IAuthService
    {

        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Role> _roleRepo;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserAPI _userAPI;

        private readonly string userName = "sportprog";
        private readonly string password = "i2Zu33gO$I";

        public AuthService ( IRepository<User> userRepo , IRepository<Role> roleRepo , IHttpClientFactory clientFactory , IPasswordHasher passwordHasher , IHttpContextAccessor contextAccessor , IUserAPI userAPI )
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _clientFactory = clientFactory;
            _passwordHasher = passwordHasher;
            _contextAccessor = contextAccessor;
            _userAPI = userAPI;
        }



        public async Task ChangePasswordAsync ( ChangePasswordDto command )
        {
            var nationalCode = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if ( nationalCode == null )
                return;

            User user = await _userRepo.FirstOrDefaultAsync(u => u.NationalCode.Value == nationalCode.Value);
            user.WithPassword(_passwordHasher.HashPassword(command.Password));
            _userRepo.Update(user);
            await _userRepo.SaveAsync();
        }

        public async Task<LoginResultDto> LoginAsync ( LoginDto command )
        {
            var user = await _userRepo.FirstOrDefaultAsync(
                u => u.UserName == command.UserName ,
                include: source =>
                    source.Include(u => u.Role));

            if ( user != null )
            {
                if ( !_passwordHasher.VerifyPassword(user.Password , command.Password) )
                    return LoginResultDto.Faild("رمز وارد شده اشتباه است");

                await AddClaimsAsync(user);
                return LoginResultDto.Successful();
            }

            return LoginResultDto.Faild("نام کاربری وارد شده اشتباه است");

        }

        public async Task<RegisterResultDto> RegisterAsync ( RegisterDto command )
        {
            var userApi = await _userAPI.GetUserAsync(command.NationalCode);
            if ( userApi == null )
                return RegisterResultDto.Faild("کاربر انتخاب شده در سیستم وجود ندارد");


            var role = await _roleRepo.FirstOrDefaultAsync(u => u.Title == SD.User);
            var user = new User(userApi.Name , userApi.Lastname , userApi.Mobile , userApi.Idmelli , userApi.Idmelli , _passwordHasher.HashPassword(command.Password) ,
                role.Id , command.StudentNumber , command.Gender);

            _userRepo.Add(user);
            await _userRepo.SaveAsync();
            await AddClaimsAsync(user);
            return RegisterResultDto.Successful();
        }

        private async Task AddClaimsAsync ( User user )
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name , user.Name + " " + user.Family));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier , user.NationalCode.Value));
            identity.AddClaim(new Claim(ClaimTypes.Gender , user.Gender.ToString()));

            if ( user.Role != null )
            {
                identity.AddClaim(new Claim(ClaimTypes.Role , user.Role.Title));
            }
            var principal = new ClaimsPrincipal(identity);
            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme , principal);
        }

    }
}
