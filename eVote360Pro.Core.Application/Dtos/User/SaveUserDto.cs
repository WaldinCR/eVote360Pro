
namespace eVote360Pro.Core.Application.Dtos.User
{
    public class SaveUserDto
    {
        public int Id { get; set; }
        public required string Name { get; set; } 
        public required string LastName { get; set; } 
        public required string Email { get; set; } 
        public required string UserName { get; set; } 
        public required string Password { get; set; } 
        public int Role { get; set; } 
        public bool IsActive { get; set; }

    }
}
