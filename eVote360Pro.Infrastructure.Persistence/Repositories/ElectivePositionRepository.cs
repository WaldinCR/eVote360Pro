using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Infrastructure.Persistence.Repositories
{
    public class ElectivePositionRepository : GenericRepository<ElectivePosition>, IElectivePositionRepository
    {
        public ElectivePositionRepository(eVote360ProDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            var normalized = name.Replace(" ", "").ToLower();
            return !await _dbContext.ElectivePositions
                .AnyAsync(p => p.Name.Replace(" ", "").ToLower() == normalized
                            && (!excludeId.HasValue || p.Id != excludeId.Value));
        }

        public async Task<bool> HasAssociatedCandidatesAsync(int positionId)
        {
            return await _dbContext.CandidatePositions
                .AnyAsync(cp => cp.ElectivePositionId == positionId);
        }
    }
}
