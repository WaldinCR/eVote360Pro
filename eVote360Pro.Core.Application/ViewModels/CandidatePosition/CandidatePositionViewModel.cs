namespace eVote360Pro.Core.Application.ViewModels.CandidatePosition
{
    public class CandidatePositionViewModel
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public string CandidateName { get; set; } = null!;
        public int ElectivePositionId { get; set; }
        public string ElectivePositionName { get; set; } = null!;
        public int PoliticalPartyId { get; set; }
        public bool IsAllied { get; set; }
    }
}