namespace Milestone1App.Models
{
    public class Cell
    {
        public bool IsMine { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; } // for 🚩 support
        public int NeighborNumber { get; set; }
    }
}
