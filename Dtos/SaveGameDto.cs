using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
    public class SaveGameDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public int Slot { get; set; }
    }
    public class CreateSaveGameRequest
    {
        public string Name { get; set; }
        public int Slot { get; set; }
    }
}
