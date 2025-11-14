using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Milestone1App.Models;
using Milestone1App.Data;
using Milestone1App.ViewModels;

namespace Milestone1App.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher<User> _hasher;

        public AccountController(AppDbContext db, IPasswordHasher<User> hasher)
        {
            _db = db;
            _hasher = hasher;
        }

        // -------------------------------
        // 1. LOGIN (GET)
        // -------------------------------
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // -------------------------------
        // 2. LOGIN (POST)
        // -------------------------------
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == vm.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(vm);
            }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, vm.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(vm);
            }

            // ✅ Create authentication cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                });

            // ✅ Optional: also store username in session for convenience
            HttpContext.Session.SetString("Username", user.Username);

            // Redirect to Game Index page after login
            return RedirectToAction("Index", "Game");
        }

        // -------------------------------
        // 3. LOGOUT
        // -------------------------------
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        // -------------------------------
        // 4. REGISTER (GET)
        // -------------------------------
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        // -------------------------------
        // 5. REGISTER (POST)
        // -------------------------------
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (await _db.Users.AnyAsync(u => u.Username == vm.Username))
            {
                ModelState.AddModelError(nameof(vm.Username), "Username already taken.");
                return View(vm);
            }

            if (await _db.Users.AnyAsync(u => u.Email == vm.Email))
            {
                ModelState.AddModelError(nameof(vm.Email), "Email already registered.");
                return View(vm);
            }

            var user = new User
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Sex = vm.Sex,
                Age = vm.Age,
                State = vm.State,
                Email = vm.Email,
                Username = vm.Username
            };

            user.PasswordHash = _hasher.HashPassword(user, vm.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(RegisterSuccess));
        }

        // -------------------------------
        // 6. REGISTER SUCCESS
        // -------------------------------
        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View();
        }
    }
}
