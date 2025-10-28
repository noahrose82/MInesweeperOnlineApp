using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Milestone1App.Services;
using Milestone1App.Models;
using System.Text.Json;

namespace Milestone1App.Controllers
{
    [Authorize] // ✅ Require login for all actions
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private const string SessionKey = "CurrentGame";

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var saved = HttpContext.Session.GetString(SessionKey);
            if (saved != null)
            {
                var game = JsonSerializer.Deserialize<GameState>(saved);
                return View(game);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Start([FromBody] GameSettings settings)
        {
            var username = User.Identity?.Name ?? "Guest";
            var game = _gameService.Start(settings, username);
            HttpContext.Session.SetString("CurrentGame", JsonSerializer.Serialize(game));
            return Json(game);
        }


        [HttpPost]
        public IActionResult Reveal(int row, int col)
        {
            var saved = HttpContext.Session.GetString(SessionKey);
            if (saved == null)
                return BadRequest("No active game.");

            var game = JsonSerializer.Deserialize<GameState>(saved);
            _gameService.Reveal(game!, row, col);
            HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(game));
            return Json(game);
        }
        [HttpPost]
        public IActionResult ToggleFlag(int row, int col)
        {
            var saved = HttpContext.Session.GetString("CurrentGame");
            if (saved == null)
                return BadRequest("No active game.");

            var game = JsonSerializer.Deserialize<GameState>(saved);
            _gameService.ToggleFlag(game!, row, col);
            HttpContext.Session.SetString("CurrentGame", JsonSerializer.Serialize(game));
            return Json(game);
        }

        [HttpPost]
        public IActionResult Restart()
        {
            HttpContext.Session.Remove(SessionKey);
            return RedirectToAction("Index");
        }
    }
}
