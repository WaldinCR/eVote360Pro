using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Infrastructure.Persistence.Repositories
{
    public class VoteRepository : GenericRepository<Vote>, IVoteRepository
    {
        public VoteRepository(eVote360ProDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Vote>> GetByElectionAsync(int electionId)
        {
            return await _dbContext.Set<Vote>()
                .Where(v => v.ElectionId == electionId)
                .ToListAsync();
        }
    }
}