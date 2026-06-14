using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Dtos.PoliticalLeaderAssignment
{
    public class SavePoliticalLeaderAssignmentDto
    {
        public int UserId { get; set; }
        public int PoliticalPartyId { get; set; }
    }
}