using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUserNameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsUserNameAsync(string username);
        Task<bool> ExistsEmailAsync(string email);
        Task<bool> IsTheOnlyActiveAdminAsync(int userId);
        Task<User?> LoginAsync(string userName, string password);
    }
}
