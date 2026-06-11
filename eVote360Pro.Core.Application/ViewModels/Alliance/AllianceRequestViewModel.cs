using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.ViewModels.Alliance
{
    public class AllianceRequestViewModel
    {
        public int Id { get; set; }

        public int ApplicantPartyId { get; set; }
        public string ApplicantPartyName { get; set; } = null!;

        public int ReceiverPartyId { get; set; }
        public string ReceiverPartyName { get; set; } = null!;

        public int Status { get; set; }
        public string StatusName { get; set; } = null!; 

        public string RequestDate { get; set; } = null!;
    }
}