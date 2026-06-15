using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces.Repositories;
using eVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Infrastructure.Persistence.Repositories
{
    public class PoliticalLeaderAssignmentRepository : GenericRepository<PoliticalLeaderAssignment>, IPoliticalLeaderAssignmentRepository
    {
       
        public PoliticalLeaderAssignmentRepository(eVote360ProDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<PoliticalLeaderAssignment?> GetByUserIdAsync(int userId)
        {
            return await _dbContext.Set<PoliticalLeaderAssignment>()
                .Include(a => a.PoliticalParty)
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }
    }
}