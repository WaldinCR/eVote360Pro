using eVote360Pro.Core.Application.ViewModels.PoliticalParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace eVote360Pro.Core.Application.Interfaces
{
    public interface IPoliticalPartyService
    {
        Task<List<PoliticalPartyViewModel>> GetAllViewModel();
        Task<SavePoliticalPartyViewModel?> GetByIdSaveViewModel(int id);
        Task<SavePoliticalPartyViewModel> AddAsync(SavePoliticalPartyViewModel vm);
        Task UpdateAsync(SavePoliticalPartyViewModel vm);
        Task ChangeStatusAsync(int id);
    }
}