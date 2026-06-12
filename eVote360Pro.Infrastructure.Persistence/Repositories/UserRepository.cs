using eVote360Pro.Core.Application.Dtos.User;
using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Domain.Common.Enums;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(eVote360ProDbContext context) : base(context) { }
        public async Task<User?> GetByUserNameAsync(string username)
            => await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.UserName == username);

        public async Task<User?> GetByEmailAsync(string email)
            => await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Email == email);

        public async Task<bool> ExistsUserNameAsync(string username)
        {
            return await _dbContext.Set<User>().AnyAsync(u => u.UserName == username);
        }

        public async Task<bool> ExistsEmailAsync(string email)
        {
            return await _dbContext.Set<User>().AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsTheOnlyActiveAdminAsync(int userId)
        {
            bool hasOtherActiveAdmin = await _dbContext.Set<User>()
                .AnyAsync(u => u.Id != userId && u.IsActive && u.Role == UserRol.Administrador);
            return !hasOtherActiveAdmin;
        }
        public async Task<User?> LoginAsync(string userName, string password)
        {
            string passwordEncrypt = PasswordEncryptation.ComputeSha256Hash(password);

            return await _dbContext.Set<User>().FirstOrDefaultAsync(u =>
                u.UserName == userName &&
                u.Password == passwordEncrypt &&
                u.IsActive);
        }
    }
}