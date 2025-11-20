using Microsoft.AspNetCore.Mvc;
using Milestone1App.Services;
using Milestone1App.Models;

namespace Milestone1App.Controllers
{
    [ApiController]
    [Route("api")]
    public class GameApiController : ControllerBase
    {
        private readonly IGameSaveService _saveService;

        public GameApiController(IGameSaveService saveService)
        {
            _saveService = saveService;
        }

        // -------------------------------------------------------
        // GET: api/showSavedGames
        // -------------------------------------------------------
        [HttpGet("showSavedGames")]
        public IActionResult GetAll()
        {
            // Milestone 4: NO AUTH — return ALL rows
            var results = _saveService.GetAll("");   // username ignored
            return Ok(results);
        }

        // -------------------------------------------------------
        // GET: api/showSavedGames/{id}
        // -------------------------------------------------------
        [HttpGet("showSavedGames/{id}")]
        public IActionResult GetOne(int id)
        {
            var game = _saveService.GetOne(id);
            if (game == null)
                return NotFound(new { message = "Game not found." });

            return Ok(game);
        }

        // -------------------------------------------------------
        // DELETE: api/deleteOneGame/{id}
        // -------------------------------------------------------
        [HttpDelete("deleteOneGame/{id}")]
        public IActionResult DeleteOne(int id)
        {
            _saveService.Delete(id);
            return Ok(new { message = $"Game {id} deleted." });
        }
    }
}
