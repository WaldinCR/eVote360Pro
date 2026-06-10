namespace eVote360Pro.Core.Application.Dtos.Citizen
{
    public class CitizenDto
    {
        public int Id { get; set; }
        public string Document { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}