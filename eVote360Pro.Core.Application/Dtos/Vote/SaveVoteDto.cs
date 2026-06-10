
namespace eVote360Pro.Core.Application.Dtos.Vote
{
    public class SaveVoteDto
    {
        public int ElectionId { get; set; }
        public int ElectivePositionId { get; set; }
        public int? CandidateId { get; set; }
    }
}
