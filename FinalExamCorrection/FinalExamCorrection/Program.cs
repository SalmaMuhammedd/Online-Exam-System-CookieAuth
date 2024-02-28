using FinalExamCorrection.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace FinalExamCorrection
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			//builder.Services.AddControllersWithViews();

			//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		 //   .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
		 //   {
			//    options.LoginPath = new PathString("/Account/Login");
			//    options.AccessDeniedPath = new PathString("/Home/Error");
		 //   });

			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
				options.LoginPath = new PathString("/Account/Login");
                //options.ForwardSignIn = new PathString("/Instructor/Home");
				options.AccessDeniedPath = "/Forbidden/";
            });


            builder.Services.AddDbContext<FinalOnlineExamSystemContext>(options =>
             options.UseSqlServer("Data Source=DESKTOP-0054Q2J\\SQLEXPRESS;Initial Catalog=FinalOnlineExamSystem;Integrated Security=True;Encrypt=False;"));

            builder.Services.AddHttpContextAccessor();

			builder.Services.AddControllersWithViews
			(options =>
			{
				options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
			});

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

			//app.MapControllerRoute(
			//	name: "Home", ///modify
			//	pattern: "{controller=Home}/{action=Index}/{id?}");

			// Define the default route pattern with a placeholder for the controller name
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Account}/{action=Home}/{id?}");

			app.Run();
        }
    }
}
