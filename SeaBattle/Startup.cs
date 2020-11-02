using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeaBattle.Models.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SeaBattle.Models;
using SeaBattle.Hubs;
using SeaBattle.Services;

namespace SeaBattle
{
    public class Startup
    {
        IConfiguration configuration;
        public Startup(IConfiguration c)
        {
            configuration = c;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<GameService>();
            services.AddSignalR();
            services.AddSession();
            services.AddAuthentication();
            services.AddAuthorization();
            services.AddDbContext<IdentityContext>(o =>
            {
                o.UseSqlServer(configuration["Data:Identity:ConnectionString"]);
            });
            services.AddIdentity<IdentityUser, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 5;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<IdentityContext>();

            services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: null,
                    template: "{controller=Main}/{action=Index}");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<GameHub>("/hub");
            });
        }
    }
}
