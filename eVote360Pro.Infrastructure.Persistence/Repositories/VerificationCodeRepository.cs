using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Infrastructure.Persistence.Repositories
{
    public class VerificationCodeRepository : GenericRepository<VerificationCode>, IVerificationCodeRepository
    {
        public VerificationCodeRepository(eVote360ProDbContext context) : base(context) { }

        public async Task<VerificationCode?> GetActiveCodeAsync(int citizenId, int electionId)
        {
            return await _dbContext.Set<VerificationCode>()
                .Where(v => v.CitizenId == citizenId
                         && v.ElectionId == electionId
                         && !v.IsUsed
                         && v.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(v => v.GeneratedAt)
                .FirstOrDefaultAsync();
        }

        public async Task MarkAsUsedAsync(int id)
        {
            var code = await _dbContext.Set<VerificationCode>().FindAsync(id);
            if (code != null)
            {
                code.IsUsed = true;
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<VerificationCode?> GetActiveCodeByCodeAsync(int citizenId, int electionId, string code)
        {
            return await _dbContext.Set<VerificationCode>()
                .Where(v => v.CitizenId == citizenId
                         && v.ElectionId == electionId
                         && v.Code == code)
                .OrderByDescending(v => v.GeneratedAt)
                .FirstOrDefaultAsync();
        }
    }
}
