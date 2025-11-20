namespace Milestone1App.Models
{
    public class SavedGameModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime DateSaved { get; set; }
        public string GameData { get; set; } = string.Empty;
    }
}
