using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Dtos.PoliticalParty
{
    public class SavePoliticalPartyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Acronym { get; set; } = null!;
        public string? LogoUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
