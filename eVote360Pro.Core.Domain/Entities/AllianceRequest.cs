using eVote360Pro.Core.Domain.Common;
using eVote360Pro.Core.Domain.Common.Enums;

namespace eVote360Pro.Core.Domain.Entities
{
    public class AllianceRequest : BaseEntity
    {
        public required int ApplicantPartyId { get; set; }
        public required int ReceiverPartyId { get; set; }
        public AllianceRequestStatus Status { get; set; } = AllianceRequestStatus.Pending;
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public DateTime? ResponseDate { get; set; }

        // Navigation Properties
        public PoliticalParty? ApplicantParty { get; set; }
        public PoliticalParty? ReceiverParty { get; set; }
    }
}