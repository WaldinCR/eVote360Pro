using eVote360Pro.Core.Application.Dtos.ElectivePosition;
using eVote360Pro.Core.Application.ViewModels.ElectivePosition;

namespace eVote360Pro.Core.Application.Interfaces
{
    public interface IElectivePositionService
    {
        Task<IReadOnlyList<ElectivePositionDto>> GetAllAsync();
        Task<SaveElectivePositionViewModel?> GetByIdSaveViewModelAsync(int id);
        Task<string?> AddAsync(SaveElectivePositionViewModel viewModel);
        Task<string?> UpdateAsync(SaveElectivePositionViewModel viewModel);
        Task<string?> DeleteLogicalAsync(int id);
        Task<List<ElectivePositionWithCandidatesDto>> GetPositionsWithCandidatesForVotingAsync();
    }
}