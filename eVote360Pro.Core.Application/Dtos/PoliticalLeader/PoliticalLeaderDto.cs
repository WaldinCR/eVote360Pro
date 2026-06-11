using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Dtos.PoliticalLeader
{
    public class PoliticalLeaderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string LeaderName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public int PoliticalPartyId { get; set; }
        public string PoliticalPartyName { get; set; } = null!;
        public string PoliticalPartyAcronym { get; set; } = null!;
        public bool IsLeaderActive { get; set; }
        public bool IsPartyActive { get; set; }
    }
}