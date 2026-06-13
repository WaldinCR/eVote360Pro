using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities
{
    public class PoliticalParty : BaseEntity    
    {
      public  string? Description { get; set; }= null;
      public required string Name { get; set; }
      public bool IsActive { get; set; } = true;
      public string LogoUrl { get; set; } = null!;
      public string Acronym { get; set; } = null!;
        public PoliticalLeader? PoliticalLeader { get; set; } //fk

        //Relaci
       public ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();
       public ICollection<AllianceRequest> SentAllianceRequests { get; set; } = new List<AllianceRequest>();
       public ICollection<AllianceRequest> ReceivedAllianceRequests { get; set; } = new List<AllianceRequest>();
       public ICollection<Alliance> AlliancesAsParty1 { get; set; } = new List<Alliance>();
       public ICollection<Alliance> AlliancesAsParty2 { get; set; } = new List<Alliance>();

    }
}
