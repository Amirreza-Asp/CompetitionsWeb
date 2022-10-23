using Competitions.Application;
using Competitions.Application.Authentication.Interfaces;
using Competitions.Application.Managment.Interfaces;
using Competitions.Application.Places.Interfaces;
using Competitions.Domain.Entities.Places.Repo;
using Competitions.Persistence;
using Competitions.Persistence.Authentication.Services;
using Competitions.Persistence.Data;
using Competitions.Persistence.Data.Initializer;
using Competitions.Persistence.Data.Initializer.Interfaces;
using Competitions.Persistence.Managment.Services;
using Competitions.Persistence.Places.Repo;
using Competitions.Persistence.Places.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Competitions.Framework
{
    public static class PresistenceRegistration
    {


        public static IServiceCollection AddPersistenceRegistrations ( this IServiceCollection services , IConfiguration configuration )
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var persistenceAssembly = assemblies.Where(item => item.GetName().Name != null && item.GetName().Name.Equals("Competitions.Persistence"))
                .FirstOrDefault();

            if ( persistenceAssembly == null )
                throw new Exception("Persistence Assembly not found");

            // db
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("Competitions")));

            services.AddScoped<IDbInitializer , DbInitializer>();

            // repositories
            services.AddScoped(typeof(IRepository<>) , typeof(Repository<>));
            services.AddScoped<IPlaceSportRepository , PlaceSportRepository>();

            // services
            services.AddScoped<IPlaceService , PlaceService>();
            services.AddScoped<IPasswordHasher , PasswordHasher>();
            services.AddScoped<IAuthService , AuthService>();
            services.AddScoped<IUserAPI , UserAPI>();
            services.AddScoped<IPositionAPI , PositionAPI>();
            services.AddScoped<IMatchService , MatchService>();
            services.AddScoped<ISmsService , SmsService>();


            // mediator
            services.AddMediatR(persistenceAssembly);

            return services;
        }

    }
}
