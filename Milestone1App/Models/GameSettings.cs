namespace Milestone1App.Models
{
    public class GameSettings
    {
        public int Rows { get; set; } = 9;
        public int Cols { get; set; } = 9;
        public Difficulty Difficulty { get; set; } = Difficulty.Easy;
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
}
