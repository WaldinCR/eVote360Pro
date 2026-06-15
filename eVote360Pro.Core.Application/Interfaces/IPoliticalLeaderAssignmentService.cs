using eVote360Pro.Core.Application.ViewModels.PoliticalLeaderAssignment;

namespace eVote360Pro.Core.Application.Interfaces
{
    public interface IPoliticalLeaderAssignmentService
    {
        Task<List<PoliticalLeaderAssignmentViewModel>> GetAllViewModel();
        Task<SavePoliticalLeaderAssignmentViewModel?> AddAsync(SavePoliticalLeaderAssignmentViewModel vm);
        Task<bool> DeleteAsync(int id);
        Task<bool> CheckIfUserIsAssignedAsync(int userId);
    }
}