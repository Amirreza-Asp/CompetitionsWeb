using Competitions.Application.Authentication.Interfaces;
using Competitions.Persistence.Data.Initializer.Interfaces;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.IdentityModel.Tokens.Jwt;

namespace Competitions.Web
{
    public class Startup
    {
        public IConfiguration Configuration
        {
            get;
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = "Cookies";
                config.DefaultSignInScheme = "Cookies";
                config.DefaultChallengeScheme = "OAuth";
            })
               .AddCookie("Cookies", options =>
               {
                   options.LoginPath = "/Authentication/Account/Login";
               })
               .AddOAuth("OAuth", options =>
               {
                   options.ClientId = Configuration.GetValue<String>("SSO:ClientId");
                   options.ClientSecret = Configuration.GetValue<String>("SSO:SecretId");
                   options.Scope.Add("openid");
                   options.Scope.Add("profile");
                   options.CallbackPath = "/signin-oauth";
                   options.UserInformationEndpoint = "https://sso.razi.ac.ir/api/v1/User/userinfo";
                   options.AuthorizationEndpoint = "https://sso.razi.ac.ir/oauth2/authorize";
                   options.TokenEndpoint = "https://sso.razi.ac.ir/oauth2/token";
                   options.Events = new OAuthEvents()
                   {
                       OnCreatingTicket = context =>
                       {
                           // read sso token
                           var accessToken = context.AccessToken;
                           var handler = new JwtSecurityTokenHandler();
                           var jsonToken = handler.ReadToken(accessToken);
                           var token = jsonToken as JwtSecurityToken;

                           if (token == null)
                               return Task.CompletedTask;

                           // get auth service
                           var provider = services.BuildServiceProvider();
                           var scope = provider.CreateScope();
                           var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

                           // generate app claims
                           var claims = authService.LoginWithSSOAsync(token).GetAwaiter().GetResult();

                           // add app tokens to context
                           foreach (var claim in claims)
                           {
                               context.Identity.AddClaim(claim);
                           }

                           return Task.CompletedTask;
                       }
                   };
               });

            services.AddHttpClient();
            services.AddHttpContextAccessor();


            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

            });

            Competitions.Framework.PresistenceRegistration.AddPersistenceRegistrations(services, Configuration);
            Competitions.Framework.CommonRegistrations.AddCommonRegistration(services);
            Competitions.Framework.ApplicationRegistration.AddApplicationRegistration(services);


        }

        public void Configure(WebApplication app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (!app.Environment.IsDevelopment() || true)
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
            app.UseSession();

            dbInitializer.Execute().GetAwaiter().GetResult();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                 );


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
            });

        }
    }
}
