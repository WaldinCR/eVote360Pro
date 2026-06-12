using eVote360Pro.Core.Application.Dtos.User;

namespace eVote360Pro.Core.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<SaveUserDto?> GetByIdSaveDtoAsync(int id);
        Task<SaveUserDto> AddAsync(SaveUserDto dto);
        Task UpdateAsync(SaveUserDto dto);
        Task DeleteAsync(int id);
        Task<bool> ExistsUserNameAsync(string username);
        Task<bool> ExistsEmailAsync(string email);
        Task<string?> ToggleActiveStatusAsync(int id);
        Task<UserDto?> LoginAsync(LoginDto dto);
        Task<bool> HasPoliticalPartyAssignedAsync(int userId);
    }
}
