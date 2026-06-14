using eVote360Pro.Core.Application.Dtos.PoliticalParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Dtos.Alliance
{
    public class AllianceRequestDto
    {
        public int Id { get; set; }
        public int ApplicantPartyId { get; set; }
        public string ApplicantPartyName { get; set; } = null!;
        public int ReceiverPartyId { get; set; }
        public string ReceiverPartyName { get; set; } = null!;
        public int Status { get; set; }
        public string RequestDate { get; set; } = null!;

        public PoliticalPartyDto? ApplicantParty { get; set; }
        public PoliticalPartyDto? ReceiverParty { get; set; }
    }
}