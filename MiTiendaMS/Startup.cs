using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace MiTiendaMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAuthentication(setup =>
            {
                setup.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                setup.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddOpenIdConnect(setup =>
            {
                setup.Authority = "https://demo.identityserver.io";
                setup.ClientId = "interactive.confidential";
                setup.ClientSecret = "secret";
                setup.Scope.Clear();
                setup.Scope.Add("openid");
                setup.Scope.Add("profile");
                setup.Scope.Add("api");
                setup.Scope.Add("email");
                setup.Scope.Add("offline_access");

                setup.ResponseType = "code";
                setup.UsePkce = true;

                setup.SaveTokens = true;

            }).AddCookie();

            services.AddAuthorization(setup =>
            {
                setup.AddPolicy("my-policy-1", builder =>
                {
                    builder.RequireClaim(ClaimTypes.NameIdentifier, "1");
                });
            });
            //services.AddAuthorization(setup =>
            //{
            //    setup.AddPolicy("my-policy-1", builder =>
            //    {
            //        //builder.RequireClaim(ClaimTypes.NameIdentifier, "1");
            //        builder.AddRequirements(new AuthorizationRequirement
            //        {
            //            AUProperty1 = "AUProperty1_valor",
            //            AUProperty2 = "AUProperty2_valor"
            //        });
            //    });
            //});
            //services.AddSingleton<IAuthorizationHandler, AuthorizationRequirementHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
