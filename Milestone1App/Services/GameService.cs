using Milestone1App.Models;

namespace Milestone1App.Services
{
    public class GameService : IGameService
    {
        private readonly Dictionary<int, GameState> _games = new();
        private int _nextId = 1;

        public GameState Start(GameSettings settings, string owner)
        {
            GameState g = new()
            {
                Id = _nextId++,
                OwnerUsername = owner,
                Rows = settings.Rows,
                Cols = settings.Cols,
                Difficulty = settings.Difficulty,
                Board = new Cell[settings.Rows][],
                StartTime = DateTime.UtcNow
            };

            // --------------------------------------------
            // Initialize the board (NOW setting Row & Col)
            // --------------------------------------------
            for (int r = 0; r < settings.Rows; r++)
            {
                g.Board[r] = new Cell[settings.Cols];
                for (int c = 0; c < settings.Cols; c++)
                {
                    g.Board[r][c] = new Cell
                    {
                        Row = r,
                        Col = c,
                        IsRevealed = false,
                        IsMine = false,
                        IsFlagged = false,
                        AdjacentMines = 0
                    };
                }
            }

            // --------------------------------------------
            // Determine mine count based on difficulty
            // --------------------------------------------
            int mines = settings.Difficulty switch
            {
                Difficulty.Easy => (int)(0.10 * settings.Rows * settings.Cols),
                Difficulty.Normal => (int)(0.15 * settings.Rows * settings.Cols),
                _ => (int)(0.20 * settings.Rows * settings.Cols),
            };

            g.MineCount = mines;

            // --------------------------------------------
            // Place mines randomly
            // --------------------------------------------
            var rand = new Random();
            int placed = 0;

            while (placed < mines)
            {
                int r = rand.Next(settings.Rows);
                int c = rand.Next(settings.Cols);

                if (!g.Board[r][c].IsMine)
                {
                    g.Board[r][c].IsMine = true;
                    placed++;
                }
            }

            // --------------------------------------------
            // Calculate Adjacent Mines Count
            // --------------------------------------------
            for (int r = 0; r < g.Rows; r++)
            {
                for (int c = 0; c < g.Cols; c++)
                {
                    if (g.Board[r][c].IsMine)
                        continue;

                    int count = 0;

                    for (int dr = -1; dr <= 1; dr++)
                    {
                        for (int dc = -1; dc <= 1; dc++)
                        {
                            if (dr == 0 && dc == 0) continue;

                            int rr = r + dr;
                            int cc = c + dc;

                            if (rr >= 0 && cc >= 0 && rr < g.Rows && cc < g.Cols)
                            {
                                if (g.Board[rr][cc].IsMine)
                                    count++;
                            }
                        }
                    }

                    g.Board[r][c].AdjacentMines = count;
                }
            }

            _games[g.Id] = g;
            return g;
        }

        public GameState? Get(int id) =>
            _games.TryGetValue(id, out var g) ? g : null;

        // ----------------------------------------------------
        // Flood-fill Reveal (your original logic)
        // ----------------------------------------------------
        public void Reveal(GameState g, int r, int c)
        {
            if (g == null || g.IsOver)
                return;

            var cell = g.Board[r][c];
            if (cell.IsRevealed || cell.IsFlagged)
                return;

            cell.IsRevealed = true;

            if (cell.IsMine)
            {
                g.IsOver = true;
                g.IsWin = false;
                return;
            }

            g.RevealedCount++;

            // Auto-reveal empty neighbors
            if (cell.AdjacentMines == 0)
            {
                for (int dr = -1; dr <= 1; dr++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        if (dr == 0 && dc == 0) continue;

                        int rr = r + dr;
                        int cc = c + dc;

                        if (rr >= 0 && cc >= 0 && rr < g.Rows && cc < g.Cols)
                        {
                            if (!g.Board[rr][cc].IsRevealed)
                                Reveal(g, rr, cc);
                        }
                    }
                }
            }

            int totalSafe = g.Rows * g.Cols - g.MineCount;

            if (g.RevealedCount >= totalSafe)
            {
                g.IsOver = true;
                g.IsWin = true;
            }
        }

        // ----------------------------------------------------
        // Toggle flag
        // ----------------------------------------------------
        public void ToggleFlag(GameState g, int r, int c)
        {
            if (g == null || g.IsOver)
                return;

            var cell = g.Board[r][c];

            if (cell.IsRevealed)
                return;

            cell.IsFlagged = !cell.IsFlagged;
        }

        public int ComputeScore(GameState g)
        {
            double sec = Math.Max(1, (DateTime.UtcNow - g.StartTime).TotalSeconds);
            double size = g.Rows * g.Cols;

            double diff = g.Difficulty switch
            {
                Difficulty.Easy => 1.0,
                Difficulty.Normal => 1.5,
                Difficulty.Hard => 2.0,
                _ => 1.0
            };

            return (int)Math.Round((size * diff * 100) / sec);
        }
    }
}
