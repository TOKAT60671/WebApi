namespace WebApi.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
