
namespace eVote360Pro.Core.Application.Dtos.Vote
{
    public class SaveVoteDto
    {
        public int ElectionId { get; set; }
        public int ElectivePositionId { get; set; }
        public int? CandidateId { get; set; }

        //para armar el correo de confirmación
        public string? PositionName { get; set; }
        public string? CandidateName { get; set; }
        public string? PartyName { get; set; }
    }
}
