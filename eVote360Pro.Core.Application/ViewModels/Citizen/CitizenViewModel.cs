namespace eVote360Pro.Core.Application.ViewModels.Citizen
{
    public class CitizenViewModel
    {
        public int Id { get; set; }
        public string Document { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}