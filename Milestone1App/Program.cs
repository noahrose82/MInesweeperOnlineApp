using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Milestone1App.Data;
using Milestone1App.Models;
using Milestone1App.Services;

namespace Milestone1App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSingleton<IGameSaveService, GameSaveService>();

            // -------------------------------------
            // 1. MVC Setup
            // -------------------------------------
            builder.Services.AddControllersWithViews();

            // -------------------------------------
            // 2. Database Connection
            // -------------------------------------
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("Default")));

            // -------------------------------------
            // 3. Session Support
            // -------------------------------------
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // -------------------------------------
            // 4. Authentication Setup (Cookie-Based)
            // -------------------------------------
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";          // Redirect if not logged in
                    options.AccessDeniedPath = "/Account/Login";   // Redirect on access denied
                    options.LogoutPath = "/Account/Logout";        // Optional explicit path
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                });

            // -------------------------------------
            // 5. Dependency Injection
            // -------------------------------------
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddScoped<IGameService, GameService>();

            var app = builder.Build();

            // -------------------------------------
            // 6. Middleware Pipeline
            // -------------------------------------
            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            // Authentication & Authorization must come after session
            app.UseAuthentication();
            app.UseAuthorization();

            // -------------------------------------
            // 7. Default Route
            // -------------------------------------
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
