
namespace eVote360Pro.Core.Application.ViewModels.Elector
{
    public class VoteConfirmationViewModel
    {
        public string CitizenName { get; set; } = null!;
        public string ElectionName { get; set; } = null!;
        public DateTime VotedAt { get; set; }
        public List<VoteSelectionSummary> Selections { get; set; } = new();
    }

    public class VoteSelectionSummary
    {
        public string PositionName { get; set; } = null!;
        public string CandidateName { get; set; } = null!; // "None" si aplica
    }
}
