namespace eVote360Pro.Core.Application.ViewModels.ElectivePosition
{
    public class ElectivePositionViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool HasParticipatedInElection { get; set; }
    }
}