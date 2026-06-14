using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.ViewModels.PoliticalLeader
{
    public class PoliticalLeaderViewModel
    {
        public int Id { get; set; }
        public string LeaderName { get; set; } = null!;
        public string PartyName { get; set; } = null!;
    }
}
