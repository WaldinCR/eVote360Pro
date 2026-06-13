namespace eVote360Pro.Core.Application.Dtos.CandidatePosition
{
    public class CandidatePositionDto
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public string CandidateName { get; set; } = null!;
        public int ElectivePositionId { get; set; }
        public string ElectivePositionName { get; set; } = null!;
        public int PoliticalPartyId { get; set; }
        
        // Propiedad auxiliar para saber si es propio o aliado
        public bool IsAllied { get; set; } 
    }
}