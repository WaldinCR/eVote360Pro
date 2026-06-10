using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Dtos.Vote
{
    public class VoteDto
    {
        public int Id { get; set; }
        public int ElectionId { get; set; }
        public int ElectivePositionId { get; set; }
        public int? CandidateId { get; set; }
    }
}
