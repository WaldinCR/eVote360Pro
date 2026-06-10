using System.Collections.Generic;
using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities
{
    public class ElectivePosition : BaseEntity
    {
        public required string Name { get; set; }
        
        public required string Description { get; set; } 
        
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public ICollection<CandidatePosition>? CandidatePositions { get; set; }
        
        // Nota: Cuando armen la entidad Vote
        //public ICollection<Vote>? Votes { get; set; }
    }
}