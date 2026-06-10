using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsUsernameAsync(string username);
        Task<bool> ExistsEmailAsync(string email);
        Task<bool> IsTheOnlyActiveAdminAsync(int userId);
    }
}
