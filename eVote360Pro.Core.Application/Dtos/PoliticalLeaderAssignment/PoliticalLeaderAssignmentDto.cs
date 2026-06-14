using eVote360Pro.Core.Application.Dtos.PoliticalParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Dtos.PoliticalLeaderAssignment
{
    public class PoliticalLeaderAssignmentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int PoliticalPartyId { get; set; }
        public string PoliticalPartyName { get; set; } = null!;


        public PoliticalPartyDto? PoliticalParty { get; set; }

       
        //public UserDto? User { get; set; } 
    }
}
