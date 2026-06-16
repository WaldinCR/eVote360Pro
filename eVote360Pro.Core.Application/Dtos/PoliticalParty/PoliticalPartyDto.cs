using eVote360Pro.Core.Application.Dtos.Alliance;
using eVote360Pro.Core.Application.Dtos.Candidate;
using eVote360Pro.Core.Application.Dtos.PoliticalLeaderAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Dtos.PoliticalParty
{
    public class PoliticalPartyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Acronym { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}