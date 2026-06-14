using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities
{
    public class Candidate : BaseEntity
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string PhotoUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public bool ParticipatedInElection { get; set; } = false;

        // FK
        public required int PoliticalPartyId { get; set; }

        // Navigation Properties
        public PoliticalParty? PoliticalParty { get; set; }
    }
}