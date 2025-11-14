namespace Milestone1App.Models
{
    public class Cell
    {
        // NEW — required for AJAX partial updates
        public int Row { get; set; }
        public int Col { get; set; }

        public bool IsRevealed { get; set; }
        public bool IsMine { get; set; }
        public bool IsFlagged { get; set; }

        // NEW — required for _CellPartial
        public int AdjacentMines { get; set; }

        // Backwards compatible — use your existing name if referenced
        public int NeighborNumber
        {
            get => AdjacentMines;
            set => AdjacentMines = value;
        }
    }
}
