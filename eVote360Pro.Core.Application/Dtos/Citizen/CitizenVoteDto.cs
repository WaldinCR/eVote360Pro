
namespace eVote360Pro.Core.Application.Dtos.Citizen
{
    public class CitizenVoteDto
    {
        public int Id { get; set; }
        public int CitizenId { get; set; }
        public int ElectionId { get; set; }
        public DateTime VotedAt { get; set; }
    }
}
