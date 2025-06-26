namespace WebApi.Dtos
{
    public class GObjectDto
    {
        public Guid Id { get; set; }
        public int PrefabId { get; set; }
        public int TypeIndex { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public Guid SaveGameId { get; set; }
    }
}
