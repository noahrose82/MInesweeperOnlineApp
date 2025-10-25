using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Milestone1App.Models;
using Milestone1App.Data;
using Milestone1App.ViewModels;

namespace Milestone1App.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher<User> _hasher;

        /// <summary>
        /// Constructor: injects database context and password hasher.
        /// </summary>
        public AccountController(AppDbContext db, IPasswordHasher<User> hasher)
        {
            _db = db;
            _hasher = hasher;
        }

        // -------------------------------
        // 1. Register (GET) -------------------------------

        /// <summary>
        /// Displays the login form.
        /// </summary>
        /// <returns>Login view with empty LoginViewModel.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        /// <summary>
        /// Handles login form submission. Verifies username and password against stored hash, and sets session
        /// variables on success.
        /// </summary>
        /// <param name="vm">LoginViewModel containing form data.</param>
        /// <returns>Redirect to StartGame on success, or redisplay form with errors.</returns>
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

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("StartGame", "Game");
        }

        /// <summary>
        /// Logs the user out by clearing the session and redirecting home.
        /// </summary>
        /// <returns>Redirect to Home/Index.</returns>
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Displays the registration form.
        /// </summary>
        /// <returns>Register view with empty RegisterViewModel.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        // -------------------------------
        // 2. Register (POST) -------------------------------

        /// <summary>
        /// Handles registration form submission. Validates input, checks for duplicate users, hashes password, and
        /// saves new user.
        /// </summary>
        /// <param name="vm">RegisterViewModel containing form data.</param>
        /// <returns>Redirect to RegisterSuccess on success, or redisplay form with errors.</returns>
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

        /// <summary>
        /// Displays the registration success page.
        /// </summary>
        /// <returns>RegisterSuccess view.</returns>
        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View();
        }

    }
}