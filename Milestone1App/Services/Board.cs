using Milestone1App.Models;

namespace Milestone1App.Services
{
    public class Board
    {
        public int NumRows { get; set; }
        public int NumCols { get; set; }
        public static List<List<CellModel>> Cells { get; set; }
        public int RewardsRemaining { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime SaveTime { get; set; }
        public TimeSpan CompletionTime { get; set; }
        public enum GameStatus { InProgress, Won, Lost }
        public int NumBombs { get; set; }
        public int NumRewards { get; set; }

        private List<(int, int)> _positions;
        private int _totalItems;
        private int _gridLength;
        private int _numClearedCells;
        private GameStatus _gameState;

        Random random = new Random();

        public Board(int numRows, int numCols)
        {
            NumRows = numRows;
            NumCols = numCols;
            Cells = new List<List<CellModel>>();
            InitializeCell();
            InitializeBombsAndRewards();
            _gameState = GameStatus.InProgress;
            _numClearedCells = 0;
            RewardsRemaining = 0;
            SetupBombsAndRewards();
            CalculateNumBombNeighbors();
            StartTime = DateTime.Now;
        }

        public void InitializeCell()
        {
            for (int row = 0; row < NumRows; row++)
            {
                Cells[row] = new List<CellModel>(NumRows);
                for (int col = 0; col < NumCols; col++)
                    Cells[row].Add(new CellModel
                    {
                        Row = row,
                        Column = col
                    });
            }
        }

        public void InitializeBombsAndRewards()
        {
            NumBombs = (int)(Cells.Count * 0.15);
            NumRewards = 3;
            _totalItems = NumBombs + NumRewards;
            _positions = new List<(int, int)>();
        }

        public void SetupBombsAndRewards()
        {
            var randomPositions = new HashSet<(int, int)>();
            while (randomPositions.Count < _totalItems)
            {
                var position = (Row: random.Next(0, NumRows), Col: random.Next(0, NumCols));
                randomPositions.Add(position);
            }
            _positions = randomPositions.ToList();

            for (int i = 0; i < NumBombs; i++)
                Cells[_positions[i].Item1][_positions[i].Item2].IsBomb = true;

            for (int i = NumBombs; i < _totalItems; i++)
                Cells[_positions[i].Item1][_positions[i].Item2].HasSpecialReward = true;
        }

        private bool IsCellOnBoard(int row, int col)
        {
            return row >= 0 && row < NumRows && col >= 0 && col < NumCols;
        }

        public void FloofFill(int row, int col)
        {
            if (!IsCellOnBoard(row, col))
                return;

            var cell = Cells[row][col];

            if (cell.IsVisited || cell.IsBomb)
                return;

            if (cell.IsNextToBomb)
            {
                cell.IsVisited = true;
                return;
            }

            cell.IsVisited = true;

            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    FloofFill(row + i, col + j);
                }
        }

        public void CalculateNumBombNeighbors()
        {
            for (int row = 0; row < NumRows; row++)
                for (int col = 0; col < NumCols; col++)
                    Cells[row][col].numBombNeighbors =GetNumBombNeighbors(row, col);
        }

        public int GetNumBombNeighbors(int row, int col)
        {
            var numBombNeighbors = 0;

            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    var checkedRow = row + i;
                    var checkedCol = col + j;

                    if (IsCellOnBoard(checkedRow, checkedCol) && Cells[checkedRow][checkedCol].IsBomb)
                    {
                        Cells[row][col].IsNextToBomb = true;
                        numBombNeighbors++;
                    }
                }

            return numBombNeighbors;
        }
    }
}
