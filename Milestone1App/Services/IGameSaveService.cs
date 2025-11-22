using Milestone1App.Models;

namespace Milestone1App.Services
{
    public interface IGameSaveService
    {
        void Save(GameState game);
        List<SavedGameModel> GetAll(string username);
        SavedGameModel? GetOne(int id);
        void Delete(int id);
    }
}
