using DataAccess.InterFaces;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tset
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //Utility
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddElmah(options =>
            {
                options.OnPermissionCheck = context => context.User.Identity.IsAuthenticated;
            });
            //ConfigureAuthenticationSchemeCookie(services);

        }
          
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
           
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseElmah();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                      name: "areas",
                      pattern:"{area:exists}/{controller=home}/{action=index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=home}/{action=index}/{id?}");


            });
       
        }

        public void ConfigureAuthenticationSchemeCookie(IServiceCollection services)
        {
            if (Configuration.GetSection("IdentityServer") != null && Convert.ToBoolean(Configuration["IdentityServer:Enable"]))
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "oidc";
                })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, x =>
                    {
                        x.ExpireTimeSpan = TimeSpan.FromHours(3);
                        x.Cookie.SameSite = SameSiteMode.None;
                        x.ForwardChallenge = "oidc";
                    })
                    .AddOpenIdConnect("oidc", options =>
                    {
                        options.Authority = Configuration["IdentityServer:Endpoint"];
                        options.ClientId = Configuration["IdentityServer:ClientID"];
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            NameClaimType = "name"
                        };
                        options.Events.OnTokenValidated = async (ctx) =>
                        {
                            var _userRepo = ctx.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                            //var user =  await _userRep
                        };

                    });




            }
            else
            {

            }
        }


    }
}
