using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities
{
    // Módulo: Asignación de Dirigentes Políticos
    // Relación uno a uno entre un Usuario (rol DirigentePolitico) y un PoliticalParty
    // Un dirigente solo puede estar asignado a un partido
    // Un partido solo puede tener un dirigente asignado
    public class PoliticalLeaderAssignment : BaseEntity
    {
        public required int UserId { get; set; }
        public required int PoliticalPartyId { get; set; }

        // Navigation Properties
        public User? User { get; set; }
        public PoliticalParty? PoliticalParty { get; set; }
    }
}