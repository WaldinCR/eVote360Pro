using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Domain.Interfaces
{
    public interface IElectivePositionRepository : IGenericRepository<ElectivePosition>
    {
        Task<bool> IsNameUniqueAsync(string name, int? id = null);
        Task<bool> HasAssociatedCandidatesAsync(int positionId);
    }
}