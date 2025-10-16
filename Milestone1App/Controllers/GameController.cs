using Microsoft.AspNetCore.Mvc;
using Milestone1App.Filters;

namespace Milestone1App.Controllers
{
    public class GameController : Controller
    {
        /// <summary>
        /// Displays the StartGame page. Access is restricted by SessionAuthorize: only logged-in users can reach this
        /// page.
        /// </summary>
        /// <returns>StartGame view if logged in, otherwise redirect to Login.</returns>
        [SessionAuthorize]
        public IActionResult StartGame()
        {
            // Retrieve username from session for display
            ViewBag.Username = HttpContext.Session.GetString("Username");

            return View();
        }
    }
}