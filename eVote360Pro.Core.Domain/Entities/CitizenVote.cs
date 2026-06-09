using eVote360Pro.Core.Domain.Common;
namespace eVote360Pro.Core.Domain.Entities
{
    public class CitizenVote : BaseEntity
    {
        public required int CitizenId { get; set; }
        public required int ElectionId { get; set; }
        public required DateTime VotedAt { get; set; }

        //navigation properties
    }
}
