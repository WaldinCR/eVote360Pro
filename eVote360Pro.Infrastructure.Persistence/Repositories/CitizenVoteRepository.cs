using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Infrastructure.Persistence.Repositories
{
    public class CitizenVoteRepository : GenericRepository<CitizenVote>, ICitizenVoteRepository
    {
        public CitizenVoteRepository(eVote360ProDbContext context) : base(context) { }

        public async Task<bool> HasAlreadyVotedAsync(int citizenId, int electionId)
        {
            return await _dbContext.Set<CitizenVote>()
                .AnyAsync(cv => cv.CitizenId == citizenId && cv.ElectionId == electionId);
        }
    }
}