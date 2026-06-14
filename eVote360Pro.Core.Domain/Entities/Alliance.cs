using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities
{
    public class Alliance : BaseEntity
    {
        public required int Party1Id { get; set; }
        public required int Party2Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public PoliticalParty? Party1 { get; set; }
        public PoliticalParty? Party2 { get; set; }
    }
}