using Microsoft.AspNetCore.Mvc;
using Milestone1App.Models;
using Milestone1App.Services;
using System.Text.Json;

namespace Milestone1App.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IGameSaveService _saveService;

        private const string SessionKey = "CurrentGame";

        public GameController(IGameService gameService, IGameSaveService saveService)
        {
            _gameService = gameService;
            _saveService = saveService;
        }

        // ---------------------------------------------------------
        // MAIN PAGE – loads initial view (settings + board section)
        // ---------------------------------------------------------
        public IActionResult Index()
        {
            // Load existing game from session (if any)
            var json = HttpContext.Session.GetString(SessionKey);

            if (!string.IsNullOrEmpty(json))
            {
                var game = JsonSerializer.Deserialize<GameState>(json);
                return View(game);
            }

            return View();
        }

        // ---------------------------------------------------------
        // START GAME (AJAX)
        // ---------------------------------------------------------
        [HttpPost]
        public IActionResult Start([FromBody] GameSettings settings)
        {
            var game = _gameService.Start(settings, User.Identity?.Name ?? "guest");

            HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(game));

            return Json(game);
        }

        // ---------------------------------------------------------
        // REVEAL A CELL (AJAX)
        // ---------------------------------------------------------
        [HttpPost]
        public IActionResult Reveal(int row, int col)
        {
            var json = HttpContext.Session.GetString(SessionKey);
            if (json == null) return BadRequest();

            var game = JsonSerializer.Deserialize<GameState>(json);
            _gameService.Reveal(game, row, col);

            HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(game));

            return Json(game);
        }

        // ---------------------------------------------------------
        // TOGGLE FLAG (AJAX)
        // ---------------------------------------------------------
        [HttpPost]
        public IActionResult ToggleFlag(int row, int col)
        {
            var json = HttpContext.Session.GetString(SessionKey);
            if (json == null) return BadRequest();

            var game = JsonSerializer.Deserialize<GameState>(json);
            _gameService.ToggleFlag(game, row, col);

            HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(game));

            return Json(game);
        }

        // ---------------------------------------------------------
        // SAVE GAME (AJAX) – No redirect, returns Ok()
        // ---------------------------------------------------------
        [HttpPost]
        public IActionResult SaveGame()
        {
            var gameJson = HttpContext.Session.GetString(SessionKey);

            if (string.IsNullOrEmpty(gameJson))
                return BadRequest(new { message = "No active game to save." });

            var game = JsonSerializer.Deserialize<GameState>(gameJson);

            _saveService.Save(game);

            return Ok(new { message = "Saved successfully" });
        }
    }
}
