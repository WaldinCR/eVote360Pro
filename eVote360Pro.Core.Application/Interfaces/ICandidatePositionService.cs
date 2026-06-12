using eVote360Pro.Core.Application.Dtos.CandidatePosition;
using eVote360Pro.Core.Application.ViewModels.CandidatePosition;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Interfaces
{
    public interface ICandidatePositionService
    {
        Task<IReadOnlyList<CandidatePositionDto>> GetAllByPartyAsync(int partyId);
        Task<string?> AddAsync(SaveCandidatePositionViewModel viewModel);
        Task<string?> DeleteAsync(int id);
    }
}