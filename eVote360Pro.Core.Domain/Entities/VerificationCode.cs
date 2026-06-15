using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities
{
    public class VerificationCode : BaseEntity
    {
        public required int CitizenId { get; set; } //fk
        public required int ElectionId { get; set; } //fk
        public required string Code { get; set; }
        public required DateTime GeneratedAt { get; set; }
        public required DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;

        //navigation properties
        public Citizen? Citizen { get; set; }
        public Election? Election { get; set; }
    }

}

