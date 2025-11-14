using Milestone1App.Models;

namespace Milestone1App.Services
{
    public interface IGameService
    {
        /// <summary>
        /// Starts a new game with the given settings and owner name.
        /// </summary>
        GameState Start(GameSettings settings, string owner);

        /// <summary>
        /// Retrieves a game by its ID (for in-memory store).
        /// </summary>
        GameState? Get(int id);

        /// <summary>
        /// Reveals a cell on the board.
        /// </summary>
        void Reveal(GameState game, int row, int col);

        /// <summary>
        /// Toggles a flag 🚩 on a cell.
        /// </summary>
        void ToggleFlag(GameState game, int row, int col);

        /// <summary>
        /// Calculates the player's score.
        /// </summary>
        int ComputeScore(GameState game);
    }
}
