using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Milestone1App.Services;
using Milestone1App.Models;
using System.Text.Json;

namespace Milestone1App.Controllers
{
    [Authorize] // Require login
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private const string SessionKey = "CurrentGame";

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // ---------------------------------------------------------
        // INDEX
        // ---------------------------------------------------------
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

        // ---------------------------------------------------------
        // START A NEW GAME
        // ---------------------------------------------------------
        [HttpPost]
        public IActionResult Start([FromBody] GameSettings settings)
        {
            var username = User.Identity?.Name ?? "Guest";
            var game = _gameService.Start(settings, username);

            HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(game));
            return Json(game);
        }

        // ---------------------------------------------------------
        // ORIGINAL JSON REVEAL (you keep this)
        // ---------------------------------------------------------
        [HttpPost]
        public IActionResult Reveal(int row, int col)
        {
            var saved = HttpContext.Session.GetString(SessionKey);
            if (saved == null)
                return BadRequest("No active game.");

            var game = JsonSerializer.Deserialize<GameState>(saved);
            _gameService.Reveal(game!, row, col);

            if (game!.IsOver)
            {
                game.EndTime = DateTime.UtcNow;

                var timeTaken = (game.EndTime.Value - game.StartTime).TotalSeconds;
                double boardFactor = game.Rows * game.Cols;

                double difficultyMultiplier = game.Difficulty switch
                {
                    Difficulty.Easy => 1.0,
                    Difficulty.Normal => 1.5,
                    Difficulty.Hard => 2.0,
                    _ => 1.0
                };

                double timeFactor = 1000 / (timeTaken + 1);
                game.Score = Math.Round(boardFactor * difficultyMultiplier * timeFactor, 2);
            }

            HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(game));
            return Json(game);
        }

        // ---------------------------------------------------------
        // ORIGINAL JSON FLAG (you keep this)
        // ---------------------------------------------------------
        [HttpPost]
        public IActionResult ToggleFlag(int row, int col)
        {
            var saved = HttpContext.Session.GetString(SessionKey);
            if (saved == null)
                return BadRequest("No active game.");

            var game = JsonSerializer.Deserialize<GameState>(saved);
            _gameService.ToggleFlag(game!, row, col);

            HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(game));
            return Json(game);
        }

        // ---------------------------------------------------------
        // AJAX PARTIAL – Reveal ONE cell (Milestone 3)
        // ---------------------------------------------------------
        [HttpPost]
        public IActionResult RevealCell(int row, int col)
        {
            var saved = HttpContext.Session.GetString(SessionKey);
            if (saved == null)
                return BadRequest("No active game.");

            var game = JsonSerializer.Deserialize<GameState>(saved);

            _gameService.Reveal(game!, row, col);

            HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(game));

            var cell = game!.Board[row][col];
            return PartialView("_CellPartial", cell);
        }

        // ---------------------------------------------------------
        // AJAX PARTIAL – Flag ONE cell (Milestone 3)
        // ---------------------------------------------------------
        [HttpPost]
        public IActionResult FlagCell(int row, int col)
        {
            var saved = HttpContext.Session.GetString(SessionKey);
            if (saved == null)
                return BadRequest("No active game.");

            var game = JsonSerializer.Deserialize<GameState>(saved);

            _gameService.ToggleFlag(game!, row, col);

            HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(game));

            var cell = game!.Board[row][col];
            return PartialView("_CellPartial", cell);
        }

        // ---------------------------------------------------------
        // RESTART GAME
        // ---------------------------------------------------------
        [HttpPost]
        public IActionResult Restart()
        {
            HttpContext.Session.Remove(SessionKey);
            return RedirectToAction("Index");
        }
    }
}
