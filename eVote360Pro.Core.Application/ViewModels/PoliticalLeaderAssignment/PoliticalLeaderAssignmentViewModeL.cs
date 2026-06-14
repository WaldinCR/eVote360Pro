namespace eVote360Pro.Core.Application.ViewModels.PoliticalLeaderAssignment
{
    public class PoliticalLeaderAssignmentViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!; // Traemos el nombre del usuario
        public int PoliticalPartyId { get; set; }
        public string PoliticalPartyName { get; set; } = null!; // Traemos el nombre del partido
    }
}