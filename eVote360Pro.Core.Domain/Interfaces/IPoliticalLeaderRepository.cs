using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories
{
    public interface IPoliticalLeaderRepository : IGenericRepository<PoliticalLeader> 
    {
        Task<PoliticalLeader?> GetByUserIdAsync(int userId);
    }
}