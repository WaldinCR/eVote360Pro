
namespace eVote360Pro.Core.Application.ViewModels.Elector
{
    public class ElectivePositionVoteViewModel
    {
        public int ElectivePositionId { get; set; }
        public required string PositionName { get; set; }
        public List<CandidateOptionViewModel> Candidates { get; set; } = new();
        public int? SelectedCandidateId { get; set; } 
    }
    public class CandidateOptionViewModel
    {
        public int? Id { get; set; } 
        public required string FullName { get; set; }
        public required string PartyName { get; set; }
    }

    public class VotacionViewModel
    {
        public List<ElectivePositionVoteViewModel> Positions { get; set; } = new();
    }
}
