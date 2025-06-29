using Dapper;
using WebApi.Dtos;
using Microsoft.Data.SqlClient;
using WebApi.Interfaces;
using WebApi.Services;

namespace WebApi.Services
{
    public class DatabaseCommunicationService(string sqlConnectionString) : IDatabaseRepository
    {
        public async Task InsertUser(UserDto user)
        {
            using var sqlConnection = new SqlConnection(sqlConnectionString);
            await sqlConnection.ExecuteAsync(
                "INSERT INTO [GUsers] (Id, UserName, Password) VALUES (@Id, @UserName, @Password)",
                user
            );
        }

        public async Task<UserDto?> GetUserByUserName(string userName)
        {
            using var sqlConnection = new SqlConnection(sqlConnectionString);
            return await sqlConnection.QueryFirstOrDefaultAsync<UserDto>(
                "SELECT Id, UserName, Password FROM [GUsers] WHERE UserName = @userName",
                new { userName }
            );
        }

        public async Task InsertNewSaveGame(SaveGameDto saveGame, Guid userId)
        {
            saveGame.UserId = userId;
            using var sqlConnection = new SqlConnection(sqlConnectionString);
            await sqlConnection.ExecuteAsync(
                "INSERT INTO [SaveGame] (Id, Name, UserId, Slot) VALUES (@Id, @Name, @UserId, @Slot)",
                saveGame
            );
        }

        public async Task<IEnumerable<SaveGameDto>> GetSaveGames(Guid userId)
        {
            using var sqlConnection = new SqlConnection(sqlConnectionString);
            return await sqlConnection.QueryAsync<SaveGameDto>(
                "SELECT Id, Name, UserId, Slot FROM [SaveGame] WHERE UserId = @userId",
                new { userId }
            );
        }

        public async Task<IEnumerable<GObjectDto>> GetgObjects(Guid saveGameId)
        {
            using var sqlConnection = new SqlConnection(sqlConnectionString);
            return await sqlConnection.QueryAsync<GObjectDto>(
                "SELECT Id, TypeIndex, PositionX, PositionY, SaveGameId FROM [GObject] WHERE SaveGameId = @saveGameId",
                new { saveGameId }
            );
        }

        public async Task InsertgObjects(GObjectDto[] gObjects, Guid saveGameId)
        {
            using var sqlConnection = new SqlConnection(sqlConnectionString);
            await sqlConnection.ExecuteAsync(
                "DELETE FROM [GObject] WHERE SaveGameId = @saveGameId",
                new { saveGameId }
            );
            if (gObjects.Length > 0)
            {
                // Ensure all objects have the correct SaveGameId
                foreach (var obj in gObjects)
                    obj.SaveGameId = saveGameId;

                await sqlConnection.ExecuteAsync(
                    "INSERT INTO [GObject] (Id, TypeIndex, PositionX, PositionY, SaveGameId) " +
                    "VALUES (@Id, @TypeIndex, @PositionX, @PositionY, @SaveGameId)",
                    gObjects
                );
            }
        }

        public async Task DeleteSaveGame(Guid saveGameId)
        {
            using var sqlConnection = new SqlConnection(sqlConnectionString);
            await sqlConnection.ExecuteAsync(
                "DELETE FROM [GObject] WHERE SaveGameId = @saveGameId; DELETE FROM [SaveGame] WHERE Id = @saveGameId",
                new { saveGameId }
            );
        }
    }
}
