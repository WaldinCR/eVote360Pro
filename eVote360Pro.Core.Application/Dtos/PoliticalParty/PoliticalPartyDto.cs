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

       public PoliticalLeaderAssignmentDto? PoliticalLeaderAssignment { get; set; }
        public ICollection<CandidateDto> Candidates { get; set; } = new List<CandidateDto>();
        public ICollection<AllianceRequestDto> SentAllianceRequests { get; set; } = new List<AllianceRequestDto>();
        public ICollection<AllianceRequestDto> ReceivedAllianceRequests { get; set; } = new List<AllianceRequestDto>();
        public ICollection<AllianceDto> AlliancesAsParty1 { get; set; } = new List<AllianceDto>();
        public ICollection<AllianceDto> AlliancesAsParty2 { get; set; } = new List<AllianceDto>();
    }
}