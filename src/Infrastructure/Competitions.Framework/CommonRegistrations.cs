using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Framework
{
    public static class CommonRegistrations
    {

        public static IServiceCollection AddCommonRegistration(this IServiceCollection services)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var commonAssembly = assemblies.Where(item => item.GetName().Name != null && item.GetName().Name.Equals("Competitions.Common"))
                .FirstOrDefault();

            if (commonAssembly == null)
                throw new Exception("Common Assembly not found");

            services.AddAutoMapper(commonAssembly);
            return services;
        }

    }
}
