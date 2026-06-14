using eVote360Pro.Core.Application.ViewModels.Candidate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Interfaces
{
    public interface ICandidateService
    {
        Task<List<CandidateViewModel>> GetAllViewModel();
        Task<List<CandidateViewModel>> GetAllByPartyIdViewModel(int partyId);

        Task<SaveCandidateViewModel?> GetByIdSaveViewModel(int id);
        Task AddAsync(SaveCandidateViewModel vm);
        Task UpdateAsync(SaveCandidateViewModel vm);
        Task ChangeStatusAsync(int id, bool status);
    }
}
