
namespace eVote360Pro.Core.Application.Dtos.VerificationCode
{
    public class VerificationCodeDto
    {
        public int Id { get; set; }
        public int CitizenId { get; set; }
        public int ElectionId { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
    }
}
