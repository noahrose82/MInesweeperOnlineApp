using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Milestone1App.Models;
using Milestone1App.Services;
using System.Text.Json;

namespace Milestone1App.Controllers
{
    [Authorize]
    public class SavedGamesController : Controller
    {
        private readonly IGameSaveService _saveService;

        public SavedGamesController(IGameSaveService saveService)
        {
            _saveService = saveService;
        }

        public IActionResult Index()
        {
            string username = User.Identity?.Name ?? "";
            var games = _saveService.GetAll(username);
            return View(games);
        }

        public IActionResult Load(int id)
        {
            var saved = _saveService.GetOne(id);
            if (saved == null)
                return NotFound();

            var game = JsonSerializer.Deserialize<GameState>(saved.GameData);

            HttpContext.Session.SetString("CurrentGame", JsonSerializer.Serialize(game));

            return RedirectToAction("Index", "Game");
        }

        public IActionResult Delete(int id)
        {
            _saveService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
