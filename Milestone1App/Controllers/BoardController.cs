using Microsoft.AspNetCore.Mvc;
using Milestone1App.Models;
using Milestone1App.Services;

namespace Milestone1App.Controllers
{
    public class BoardController : Controller
    {
        Board board = new Board(7, 10);
        
        private static List<List<CellModel>> cells = Board.Cells;

        public IActionResult Index()
        {
            return View(cells);
        }
    }
}
