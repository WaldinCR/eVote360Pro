using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities
{
    public class PoliticalParty : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Acronym { get; set; }
        public required string LogoUrl { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public PoliticalLeaderAssignment? PoliticalLeaderAssignment { get; set; }
        public ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();
        public ICollection<AllianceRequest> SentAllianceRequests { get; set; } = new List<AllianceRequest>();
        public ICollection<AllianceRequest> ReceivedAllianceRequests { get; set; } = new List<AllianceRequest>();
        public ICollection<Alliance> AlliancesAsParty1 { get; set; } = new List<Alliance>();
        public ICollection<Alliance> AlliancesAsParty2 { get; set; } = new List<Alliance>();
    }
}