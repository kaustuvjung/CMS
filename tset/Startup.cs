using DataAccess;
using tset.Controllers;
using DataAccess.InterFaces;
using ElmahCore.Mvc;
using Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using DataAccess.Model.Setup;

namespace tset
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Utility.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddElmah(options =>
            {
                options.OnPermissionCheck = context => context.User.Identity.IsAuthenticated;
            });
            ConfigureAuthenticationSchemeCookie(services);
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options => {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            DI.Initialize(services);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            DataDependencyRegistrar.RegisterDependencies();

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

        private void ConfigureAuthenticationSchemeCookie(IServiceCollection services)
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
                    x.ExpireTimeSpan = TimeSpan.FromHours(2);
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
                        var user = await _userRepo.GetUserByUsernameAsync(ctx.Principal.Identity.Name);

                        if (user == null) return;

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Rows[0]["Name"].ToString()),
                            new Claim("Id",  user.Rows[0]["Id"].ToString()),
                            new Claim("Username", user.Rows[0]["Username"].ToString()),
                            new Claim("Department", user.Rows[0]["Department"].ToString()),
                            new Claim("Permission",  user.Rows[0]["PermissionId"].ToString())
                        };

                        var appIdentity = new ClaimsIdentity(claims);
                        ctx.Principal.AddIdentity(appIdentity);
                    };
                });
            }
            else
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Custom";
                    options.DefaultChallengeScheme = "Custom";
                })
                .AddCookie("Cookies", options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                    options.AccessDeniedPath = "/main/unauthorized";
                    options.LoginPath = "/user/login";
                    options.LogoutPath = "/user/logoff";
                    options.Cookie.Name = "__cms__";
                    //options.ExpireTimeSpan = DateTime.Now.TimeOfDay.Add(new TimeSpan(0, 1000, 0));
                })
                .AddJwtBearer("Bearer", options =>
                {
                    var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
                    var key = new SymmetricSecurityKey(secretBytes);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = Constants.Issuer,
                        ValidAudience = Constants.Audiance,
                        IssuerSigningKey = key,
                    };
                    options.RequireHttpsMetadata = false;
                })
                .AddPolicyScheme("Custom", "Custom", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        string authorization = context.Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization];
                        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                            return JwtBearerDefaults.AuthenticationScheme;

                        return CookieAuthenticationDefaults.AuthenticationScheme;
                    };
                });

            }

        }

    }
}