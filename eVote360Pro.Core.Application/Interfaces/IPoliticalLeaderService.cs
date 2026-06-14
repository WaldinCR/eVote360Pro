using eVote360Pro.Core.Application.ViewModels.PoliticalLeader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Interfaces
{
    public interface IPoliticalLeaderService
    {
        Task<List<PoliticalLeaderViewModel>> GetAllLeadersAsync();
        Task<SavePoliticalLeaderViewModel?> GetByIdSaveViewModel(int id);
        Task AddLeaderAsync(SavePoliticalLeaderViewModel vm);
        Task UpdateLeaderAsync(SavePoliticalLeaderViewModel vm);
        Task DeleteLeaderAsync(int id);
    }
}
