using eVote360Pro.Core.Application.Dtos.PoliticalParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Dtos.Alliance
{
    public class AllianceDto
    {
        public int Id { get; set; }
        public int Party1Id { get; set; }
        public string Party1Name { get; set; } = null!;
        public int Party2Id { get; set; }
        public string Party2Name { get; set; } = null!;
        public string CreationDate { get; set; } = null!;

        public PoliticalPartyDto? Party1 { get; set; }
        public PoliticalPartyDto? Party2 { get; set; }
    }
}