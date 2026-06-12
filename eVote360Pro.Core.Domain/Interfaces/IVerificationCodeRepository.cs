using eVote360Pro.Core.Domain.Entities;


namespace eVote360Pro.Core.Domain.Interfaces
{
    public interface IVerificationCodeRepository : IGenericRepository<VerificationCode>
    {
        Task<VerificationCode?> GetActiveCodeAsync(int citizenId, int electionId);
        Task<VerificationCode?> GetActiveCodeByCodeAsync(int citizenId, int electionId, string code);
        Task MarkAsUsedAsync(int id);
    }
}
