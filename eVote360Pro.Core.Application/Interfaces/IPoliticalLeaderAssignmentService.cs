using eVote360Pro.Core.Application.ViewModels.PoliticalLeaderAssignment;

namespace eVote360Pro.Core.Application.Interfaces
{
    public interface IPoliticalLeaderAssignmentService
    {
        Task<List<PoliticalLeaderAssignmentViewModel>> GetAllViewModel();
        Task AddAsync(SavePoliticalLeaderAssignmentViewModel vm);
        Task DeleteAsync(int id);
        Task<bool> CheckIfUserIsAssignedAsync(int userId);
    }
}