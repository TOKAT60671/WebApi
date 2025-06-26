using WebApi.Dtos;

namespace WebApi.Interfaces
{
    public interface IDatabaseRepository
    {
        Task InsertgObjects(GObjectDto[] gObjects, string saveGameId);
        Task InsertNewSaveGame(SaveGameDto saveGame, string id);
        Task<IEnumerable<SaveGameDto>> GetSaveGames(string id);
        Task<IEnumerable<GObjectDto>> GetgObjects(string saveGameId);
        Task DeleteSaveGame(string id);
    }
}
