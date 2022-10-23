using Competitions.Application.Sports.Interfaces;
using Competitions.Application.Sports.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Competitions.Framework
{
    public static class ApplicationRegistration
    {
        public static IServiceCollection AddApplicationRegistration ( this IServiceCollection services )
        {
            services.AddScoped<ISportTypeService , SportTypeService>();
            services.AddScoped<ISportService , SportService>();
            services.AddScoped<ICETService , CETService>();


            return services;
        }
    }
}
