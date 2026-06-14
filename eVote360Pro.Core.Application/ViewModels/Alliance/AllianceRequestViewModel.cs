using eVote360Pro.Core.Domain.Common.Enums;

namespace eVote360Pro.Core.Application.ViewModels.Alliance
{
    public class AllianceRequestViewModel
    {
        public int Id { get; set; }

        public int ApplicantPartyId { get; set; }
        public string ApplicantPartyName { get; set; } = null!;

        public int ReceiverPartyId { get; set; }
        public string ReceiverPartyName { get; set; } = null!;

        public AllianceRequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }
    }
}