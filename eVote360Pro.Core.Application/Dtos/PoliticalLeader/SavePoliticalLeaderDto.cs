using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Dtos.PoliticalLeader
{
    public class SavePoliticalLeaderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PoliticalPartyId { get; set; }
    }
}