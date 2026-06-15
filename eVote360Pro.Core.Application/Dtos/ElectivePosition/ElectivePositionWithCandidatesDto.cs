using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Dtos.ElectivePosition
{
    public class ElectivePositionWithCandidatesDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CandidateForVotingDto> Candidates { get; set; } = new();
    }
    public class CandidateForVotingDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PartyName { get; set; } = string.Empty;
    }
}
