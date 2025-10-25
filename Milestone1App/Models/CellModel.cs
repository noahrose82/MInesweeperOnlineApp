namespace Milestone1App.Models
{
    public class CellModel
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsVisited { get; set; }
        public bool IsBomb { get; set; }
        public bool IsFlagged { get; set; }
        public int numBombNeighbors { get; set; }
        public bool IsNextToBomb { get; set; }
        public bool HasSpecialReward { get; set; }
        public bool IsAlreadyCleared { get; set; }
        public bool RewardUsed { get; set; }

        public CellModel()
        {
            Row = -1;
            Column = -1;
        }



    }
}
