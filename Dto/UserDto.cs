namespace WebAPI_tutorial.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();// Guid --> Universally Unique Identifier 
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
