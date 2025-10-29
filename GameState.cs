namespace Milestone1App.Models
{
    public class GameState
    {
        public int Id { get; set; }
        public string OwnerUsername { get; set; } = string.Empty;
        public int Rows { get; set; }
        public int Cols { get; set; }
        public Difficulty Difficulty { get; set; }
        public Cell[][] Board { get; set; } = Array.Empty<Cell[]>(); // jagged for JSON
        public int MineCount { get; set; }
        public int RevealedCount { get; set; }
        public bool IsOver { get; set; }
        public bool IsWin { get; set; }
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime? EndTime { get; set; }
        public double ElapsedSeconds => EndTime.HasValue ? (EndTime.Value - StartTime).TotalSeconds : 0;
        public double Score { get; set; }
    }
}
