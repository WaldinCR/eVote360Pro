using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities
{
    public class Election : BaseEntity
    {
        public required string Name { get; set; }
        public required int Year { get; set; }
        public required ElectionStatus Status { get; set; }

        public DateTime? ActivationDate { get; set; }
        public DateTime? FinishedDate { get; set; }

        // Navigation Properties
        //public ICollection<Vote>? Votes { get; set; }
        //public ICollection<CitizenVote>? CitizenVotes { get; set; }
    }
}
