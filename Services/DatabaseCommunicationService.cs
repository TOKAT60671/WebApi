using Dapper;
using WebApi.Dtos;
using Microsoft.Data.SqlClient;
using WebApi.Interfaces;
using WebApi.Services;

namespace WebApi.Services
{
    public class DatabaseCommunicationService(string connectionString) : IDatabaseRepository
    {
        // Create a new 2D world for a user
        public async Task InsertNewSaveGame(SaveGameDto saveGame, string userId)
        {
            saveGame.UserId = userId;
            using var sqlConnection = new SqlConnection(connectionString);
            await sqlConnection.ExecuteAsync(
                "INSERT INTO [SaveGame] (Id, Name, UserId) VALUES (@Id, @Name, @UserId)",
                saveGame
            );
        }

        // List all 2D worlds for a user
        public async Task<IEnumerable<SaveGameDto>> GetSaveGames(string userId)
        {
            using var sqlConnection = new SqlConnection(connectionString);
            return await sqlConnection.QueryAsync<SaveGameDto>(
                "SELECT * FROM [SaveGame] WHERE UserId = @userId",
                new { userId }
            );
        }

        // View all objects in a specific 2D world
        public async Task<IEnumerable<GObjectDto>> GetgObjects(string saveGameId)
        {
            using var sqlConnection = new SqlConnection(connectionString);
            return await sqlConnection.QueryAsync<GObjectDto>(
                "SELECT * FROM [GObject] WHERE SaveGameId = @saveGameId",
                new { saveGameId }
            );
        }

        // Add objects to a 2D world (replaces all objects for that world)
        public async Task InsertgObjects(GObjectDto[] gObjects, string saveGameId)
        {
            using var sqlConnection = new SqlConnection(connectionString);
            await sqlConnection.ExecuteAsync("DELETE FROM [GObject] WHERE SaveGameId = @saveGameId", new { saveGameId });
            if (gObjects.Length > 0)
            {
                await sqlConnection.ExecuteAsync(
                    "INSERT INTO [GObject] (Id, PrefabId, TypeIndex, PositionX, PositionY, SaveGameId) " +
                    "VALUES (@Id, @PrefabId, @TypeIndex, @PositionX, @PositionY, @SaveGameId)",
                    gObjects
                );
            }
        }

        // Delete a 2D world and its objects
        public async Task DeleteSaveGame(string saveGameId)
        {
            using var sqlConnection = new SqlConnection(connectionString);
            await sqlConnection.ExecuteAsync(
                "DELETE FROM [GObject] WHERE SaveGameId = @saveGameId; DELETE FROM [SaveGame] WHERE Id = @saveGameId",
                new { saveGameId }
            );
        }
    }
}
