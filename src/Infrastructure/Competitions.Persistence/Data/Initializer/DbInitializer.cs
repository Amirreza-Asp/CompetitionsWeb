using Competitions.Application.Authentication.Interfaces;
using Competitions.Common;
using Competitions.Domain.Entities.Authentication;
using Competitions.Persistence.Data.Initializer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Competitions.Persistence.Data.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserAPI _userApi;
        private readonly IPasswordHasher _hasher;

        public DbInitializer(ApplicationDbContext db, IUserAPI userApi, IPasswordHasher hasher)
        {
            _db = db;
            _userApi = userApi;
            _hasher = hasher;
        }

        public async Task Execute()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    await _db.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {

            }


            if (!_db.Role.Any(u => u.Title == SD.Admin))
            {
                _db.Role.Add(new Role(SD.Admin, "ادمین", "دسترسی کامل سیستم"));
                _db.Role.Add(new Role(SD.Publisher, "ناشر", "دسترسی به تمام اطلاعات به جز تعیین عضو کمیته"));
                _db.Role.Add(new Role(SD.User, "کاربر", "بدون دسترسی"));
            }
            else
            {
                return;
            }

            await _db.SaveChangesAsync();

            var admin = _db.Role.First(u => u.Title == SD.Admin);
            var user = await _userApi.GetUserAsync(SD.DefaultNationalCode);

            _db.User.Add(new User(user.name, user.lastname, user.mobile, user.idmelli, user.idmelli,
                _hasher.HashPassword(user.idmelli), admin.Id, user.student_number.ToString(), user.trend, user.isMale < 1, user.type));

            await _db.SaveChangesAsync();
        }
    }
}
