using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Domain.Interfaces
{
    public interface ICitizenVoteRepository : IGenericRepository<CitizenVote>
    {
        Task<bool> HasAlreadyVotedAsync(int citizenId, int electionId);
    }
}
