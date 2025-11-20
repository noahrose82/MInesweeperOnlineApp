using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Milestone1App.Models;
using Newtonsoft.Json;

namespace Milestone1App.Services
{
    public interface IGameSaveService
    {
        void Save(GameState game);
        List<SavedGameModel> GetAll(string username);
        SavedGameModel? GetOne(int id);
        void Delete(int id);
    }

    public class GameSaveService : IGameSaveService
    {
        private readonly string _conn;

        public GameSaveService(IConfiguration config)
        {
            _conn = config.GetConnectionString("Default");
        }

        // ---------------------------
        // SAVE GAME
        // ---------------------------
        public void Save(GameState game)
        {
            string json = JsonConvert.SerializeObject(game);

            using SqlConnection conn = new SqlConnection(_conn);
            conn.Open();

            string sql = @"INSERT INTO Games (UserName, DateSaved, GameData)
                           VALUES (@u, @d, @g)";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", game.OwnerUsername);
            cmd.Parameters.AddWithValue("@d", DateTime.Now);
            cmd.Parameters.AddWithValue("@g", json);

            cmd.ExecuteNonQuery();
        }

        // ---------------------------
        // GET ALL SAVED GAMES
        // ---------------------------
        public List<SavedGameModel> GetAll(string username)
        {
            List<SavedGameModel> list = new();

            using SqlConnection conn = new SqlConnection(_conn);
            conn.Open();

            string sql = "SELECT * FROM Games WHERE UserName = @u ORDER BY DateSaved DESC";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", username);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new SavedGameModel
                {
                    Id = (int)reader["Id"],
                    UserName = reader["UserName"].ToString() ?? "",
                    DateSaved = (DateTime)reader["DateSaved"],
                    GameData = reader["GameData"].ToString() ?? ""
                });
            }

            return list;
        }

        // ---------------------------
        // GET ONE SAVED GAME
        // ---------------------------
        public SavedGameModel? GetOne(int id)
        {
            using SqlConnection conn = new SqlConnection(_conn);
            conn.Open();

            string sql = "SELECT * FROM Games WHERE Id = @id";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using SqlDataReader r = cmd.ExecuteReader();

            if (!r.Read())
                return null;

            return new SavedGameModel
            {
                Id = (int)r["Id"],
                UserName = r["UserName"].ToString() ?? "",
                DateSaved = (DateTime)r["DateSaved"],
                GameData = r["GameData"].ToString() ?? ""
            };
        }

        // ---------------------------
        // DELETE SAVED GAME
        // ---------------------------
        public void Delete(int id)
        {
            using SqlConnection conn = new SqlConnection(_conn);
            conn.Open();

            string sql = "DELETE FROM Games WHERE Id = @id";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
