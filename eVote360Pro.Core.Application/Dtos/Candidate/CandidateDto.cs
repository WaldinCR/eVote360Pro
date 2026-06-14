using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eVote360Pro.Core.Application.Dtos.PoliticalParty;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Dtos.Candidate
{
    public class CandidateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhotoUrl { get; set; } = null!;
        public bool IsActive { get; set; }
        public int PoliticalPartyId { get; set; }
        public string PoliticalPartyName { get; set; } = null!;
        public PoliticalPartyDto? PoliticalParty { get; set; }
    }
}