using Microsoft.AspNetCore.Mvc;

namespace Milestone1App.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Displays the home page (default route).
        /// </summary>
        /// <returns>Index view.</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
