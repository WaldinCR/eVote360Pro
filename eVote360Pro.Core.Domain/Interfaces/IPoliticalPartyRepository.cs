using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories
{
    public interface IPoliticalPartyRepository : IGenericRepository<PoliticalParty>
    {
        Task<PoliticalParty?> GetByAcronymAsync(string acronym);
    }
}