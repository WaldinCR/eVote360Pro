using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities
    {
    public class CandidatePosition : BaseEntity 
    {
        public required int CandidateId { get; set; }

        public required int ElectivePositionId { get; set; }

        public required int PoliticalPartyId { get; set; } 

        // Nav
        //public Candidate? Candidate { get; set; }

        public ElectivePosition? ElectivePosition { get; set; }

        //public PoliticalParty? PoliticalParty { get; set; }
    }
}