using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces.Repositories;
using eVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Infrastructure.Persistence.Repositories
{
    public class PoliticalLeaderRepository : GenericRepository<PoliticalLeader>, IPoliticalLeaderRepository
    {
        public PoliticalLeaderRepository(eVote360ProDbContext context) : base(context) { }

        public async Task<PoliticalLeader?> GetByUserIdAsync(int userId)
        {
            return await _dbContext.Set<PoliticalLeader>()
                .Include(pl => pl.PoliticalParty)
                .FirstOrDefaultAsync(pl => pl.UserId == userId);
        }
    }
}
