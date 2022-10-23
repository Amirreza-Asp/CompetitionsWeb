using Competitions.Framework;
using Competitions.Persistence.Data.Initializer.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Competitions.Web
{
	public class Startup
	{
		public IConfiguration Configuration
		{
			get;
		}

		public Startup ( IConfiguration configuration )
		{
			Configuration = configuration;
		}

		public void ConfigureServices ( IServiceCollection services , IWebHostEnvironment env )
		{
			services.AddControllersWithViews()
				.AddRazorRuntimeCompilation();


			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			   .AddCookie(options =>
			   {
				   options.Cookie.HttpOnly = false;
				   options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
				   options.LoginPath = "/Authentication/Account/Login";
				   options.AccessDeniedPath = "/Home/AccessDenied";
				   options.SlidingExpiration = true;
			   });

			services.AddHttpClient();
			services.AddHttpContextAccessor();


			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(60);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});


			services.AddPersistenceRegistrations(Configuration)
				.AddCommonRegistration()
				.AddApplicationRegistration();
		}

		public void Configure ( WebApplication app , IWebHostEnvironment env , IDbInitializer dbInitializer )
		{
			if ( !app.Environment.IsDevelopment() )
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			dbInitializer.Execute();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					  name: "areas" ,
					  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
				 );

				endpoints.MapControllerRoute(
					name: "default" ,
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});

		}
	}
}
