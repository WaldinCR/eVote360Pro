using eVote360Pro.Core.Application.Dtos.Citizen;
using eVote360Pro.Core.Application.ViewModels.Citizen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Interfaces
{
    public interface ICitizenService
    {
        Task<IReadOnlyList<CitizenDto>> GetAllAsync();
        Task<SaveCitizenViewModel?> GetByIdSaveViewModelAsync(int id);
        Task<string?> AddAsync(SaveCitizenViewModel viewModel);
        Task<string?> UpdateAsync(SaveCitizenViewModel viewModel);
        Task<string?> DeleteLogicalAsync(int id);
    }
}