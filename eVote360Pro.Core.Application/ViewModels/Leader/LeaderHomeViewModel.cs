using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Leader
{
    public class LeaderHomeViewModel
    {
        public int TotalCandidates { get; set; }
        public int ActiveCandidates { get; set; }
        public int InactiveCandidates { get; set; }
        public int ActiveAlliances { get; set; }
        public int PendingAllianceRequests { get; set; }
    }
}