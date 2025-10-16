using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Milestone1App.Data;
using Milestone1App.Models;
using System;

namespace Milestone1App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add MVC controllers and views
            builder.Services.AddControllersWithViews();

            // Register EF Core DbContext with SQL Server
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("Default")));

            // Enable session support (needed for login state)
            builder.Services.AddSession();

            // Add password hasher for secure password hashing/salting
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // No full Identity auth, but keep session + custom check
            app.UseSession();

            app.UseAuthorization();

            // Default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}