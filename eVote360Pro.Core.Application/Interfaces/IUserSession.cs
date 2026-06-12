using eVote360Pro.Core.Application.ViewModels.User;


namespace eVote360Pro.Core.Application.Interfaces
{
    public interface IUserSession
    {
        UserViewModel? GetUserSession();
        bool HasUser();
        bool IsAdmin();
    }
}
