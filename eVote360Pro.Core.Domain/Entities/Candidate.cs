using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eVote360Pro.Core.Domain.Common;


namespace eVote360Pro.Core.Domain.Entities
{
    public class Candidate : BaseEntity
    {
        public required string Name { get; set; } = null!;
        public required string LastName { get; set; } = null!;  
        public required string PhotoUrl { get; set; } = null!;  
        public bool IsActive { get; set; } = true;

        //partido politico relaci
        public required int PoliticalPartyId { get; set; }
        public PoliticalParty PoliticalParty { get; set; } = null!;
        public ICollection<CandidatePosition>? CandidatePositions { get; set; }

    }
}
