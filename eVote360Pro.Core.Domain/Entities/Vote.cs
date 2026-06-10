using eVote360Pro.Core.Domain.Common;
namespace eVote360Pro.Core.Domain.Entities
{
    public class Vote : BaseEntity
    {
        public required int ElectionId { get; set; }
        public required int ElectivePositionId { get; set; }
        public int? CandidateId { get; set; }

        //navigation properties
        // public Eleccion? Eleccion { get; set; }          
        // public PuestoElectivo? PuestoElectivo { get; set; } 
        // public Candidato? Candidato { get; set; }        
    }

}

