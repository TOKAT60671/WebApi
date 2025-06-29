using WebApi.Dtos;

namespace WebApi.Interfaces
{
    public interface IDatabaseRepository
    {
        Task InsertUser(UserDto user);
        Task<UserDto?> GetUserByUserName(string userName);

        Task InsertNewSaveGame(SaveGameDto saveGame, Guid userId);
        Task<IEnumerable<SaveGameDto>> GetSaveGames(Guid userId);

        Task<IEnumerable<GObjectDto>> GetgObjects(Guid saveGameId);
        Task InsertgObjects(GObjectDto[] gObjects, Guid saveGameId);

        Task DeleteSaveGame(Guid saveGameId);
    }
}
