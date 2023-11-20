﻿using Competitions.Application;
using Competitions.Application.Authentication.Interfaces;
using Competitions.Common;
using Competitions.Domain.Dtos.Authentication.User;
using Competitions.Domain.Entities.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
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

        public AuthService(IRepository<User> userRepo, IRepository<Role> roleRepo, IHttpClientFactory clientFactory, IPasswordHasher passwordHasher, IHttpContextAccessor contextAccessor, IUserAPI userAPI)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _clientFactory = clientFactory;
            _passwordHasher = passwordHasher;
            _contextAccessor = contextAccessor;
            _userAPI = userAPI;
        }



        public async Task ChangePasswordAsync(ChangePasswordDto command)
        {
            var nationalCode = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (nationalCode == null)
                return;

            User user = await _userRepo.FirstOrDefaultAsync(u => u.NationalCode.Value == nationalCode.Value);
            user.WithPassword(_passwordHasher.HashPassword(command.Password));
            _userRepo.Update(user);
            await _userRepo.SaveAsync();
        }

        public async Task<LoginResultDto> LoginAsync(LoginDto command)
        {
            var user = await _userRepo.FirstOrDefaultAsync(
                u => u.UserName == command.UserName && u.Type != SD.ExtraType,
                include: source =>
                    source.Include(u => u.Role));

            if (user != null)
            {
                if (!_passwordHasher.VerifyPassword(user.Password, command.Password))
                    return LoginResultDto.Faild("رمز وارد شده اشتباه است");

                if (String.IsNullOrEmpty(user.Type))
                {
                    var userAPI = await _userAPI.GetUserAsync(user.NationalCode);
                    if (userAPI != null)
                    {
                        user.WithType(userAPI.type);
                        _userRepo.Update(user);
                        await _userRepo.SaveAsync();

                    }

                }

                AddClaims(user);
                return LoginResultDto.Successful();
            }
            else
            {

            }

            return LoginResultDto.Faild("نام کاربری وارد شده اشتباه است");

        }

        public async Task<RegisterResultDto> RegisterAsync(RegisterDto command)
        {
            var userApi = await _userAPI.GetUserAsync(command.NationalCode);
            if (userApi == null)
                return RegisterResultDto.Faild("کاربر انتخاب شده در سیستم وجود ندارد");


            var role = await _roleRepo.FirstOrDefaultAsync(u => u.Title == SD.User);
            var user = new User(userApi.name, userApi.lastname, userApi.mobile, userApi.idmelli, userApi.idmelli, _passwordHasher.HashPassword(command.Password),
                role.Id, command.StudentNumber, userApi.trend, userApi.isMale < 1, userApi.type);

            _userRepo.Add(user);
            await _userRepo.SaveAsync();
            AddClaims(user);
            return RegisterResultDto.Successful();
        }

        private List<Claim> AddClaims(User user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.Name + " " + user.Family));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.NationalCode.Value));
            claims.Add(new Claim(ClaimTypes.Gender, user.Gender.ToString()));
            claims.Add(new Claim(ClaimTypes.Actor, user.Type.ToString()));

            if (user.Role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role.Title));
            }

            return claims;
        }

        public async Task KhemdatLoginAsync(String nationalCode)
        {
            var user = await _userRepo.FirstOrDefaultAsync(b => b.NationalCode.Value == nationalCode,
                include: source => source.Include(u => u.Role));

            if (user == null)
            {
                var userApi = await _userAPI.GetUserAsync(nationalCode);

                var role = await _roleRepo.FirstOrDefaultAsync(u => u.Title == SD.User);
                user = new User(userApi.name, userApi.lastname, userApi.mobile, userApi.idmelli, userApi.idmelli, _passwordHasher.HashPassword(nationalCode),
                role.Id, userApi.student_number.ToString(), userApi.trend, userApi.isMale < 1, userApi.type);

                _userRepo.Add(user);
                await _userRepo.SaveAsync();
            }

            AddClaims(user);
        }

        public async Task<List<Claim>> LoginWithSSOAsync(JwtSecurityToken ssoToken)
        {
            var nationalId = ssoToken.Claims.First(b => b.Type == "nationalId").Value;


            var user = await _userRepo.FirstOrDefaultAsync(
               u => u.UserName == nationalId && u.Type != SD.ExtraType,
               include: source =>
                   source.Include(u => u.Role));

            if (user != null)
            {
                if (String.IsNullOrEmpty(user.Type))
                {
                    var userAPI = await _userAPI.GetUserAsync(user.NationalCode);
                    if (userAPI != null)
                    {
                        user.WithType(userAPI.type);
                        _userRepo.Update(user);
                        await _userRepo.SaveAsync();
                    }
                }
            }
            else
            {
                var phone = ssoToken.Claims.First(b => b.Type == "phone").Value;
                var firstName = ssoToken.Claims.First(b => b.Type == "firstName").Value;
                var lastName = ssoToken.Claims.First(b => b.Type == "lastName").Value;


                var userApi = await _userAPI.GetUserAsync(nationalId);
                if (userApi == null)
                    throw new Exception("ورود با شکست مواجه شد");


                var role = await _roleRepo.FirstOrDefaultAsync(u => u.Title == SD.User);
                user = new User(userApi.name, userApi.lastname, userApi.mobile, userApi.idmelli, userApi.idmelli, _passwordHasher.HashPassword(userApi.idmelli),
                    role.Id, userApi.student_number.ToString(), userApi.trend, userApi.isMale < 1, userApi.type);

                _userRepo.Add(user);
                await _userRepo.SaveAsync();
            }


            var claims = AddClaims(user);
            return claims;
        }
    }
}
