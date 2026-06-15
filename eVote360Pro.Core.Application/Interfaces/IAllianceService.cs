using eVote360Pro.Core.Application.ViewModels.Alliance;

namespace eVote360Pro.Core.Application.Interfaces
{
    public interface IAllianceService
    {
        Task<List<AllianceViewModel>> GetAllViewModel();
        Task<bool> DeleteAsync(int id);
        Task<bool> HasActiveAllianceAsync(int party1Id, int party2Id);
    }
}