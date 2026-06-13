using eVote360Pro.Core.Application.Dtos.Election;
using eVote360Pro.Core.Application.ViewModels.Election;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Interfaces
{
    public interface IElectionService
    {
        Task<IReadOnlyList<ElectionDto>> GetAllAsync();
        Task<string?> AddAsync(SaveElectionViewModel viewModel);
        Task<string?> ActivateElectionAsync(int id);
        Task<string?> FinishElectionAsync(int id);
        Task<ElectionResultViewModel?> GetResultsAsync(int id);
    }
}