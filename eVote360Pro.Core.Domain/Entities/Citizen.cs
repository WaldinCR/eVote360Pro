using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities
{
        public class Citizen : BaseEntity
        {
        public required string IdentificationNumber { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public ICollection<CitizenVote>? CitizenVotes { get; set; }
        public ICollection<VerificationCode>? VerificationCodes { get; set; }
        }
}

