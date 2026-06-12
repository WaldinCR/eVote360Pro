using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.User;
using eVote360Pro.Core.Domain.Common.Enums;

namespace eVote360Pro.App.Middlewares
{
    public class UserSession : IUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasUser()
        {
            UserViewModel? userViewModel = _httpContextAccessor.HttpContext?
                .Session.Get<UserViewModel>("User");

            if (userViewModel == null)
            {
                return false;
            }

            return true;
        }

        public UserViewModel? GetUserSession()
        {
            UserViewModel? userViewModel = _httpContextAccessor.HttpContext?
                .Session.Get<UserViewModel>("User");

            if (userViewModel == null)
            {
                return null;
            }

            return userViewModel;
        }

        public bool IsAdmin()
        {
            UserViewModel? userViewModel = _httpContextAccessor.HttpContext?
                .Session.Get<UserViewModel>("User");

            if (userViewModel == null)
            {
                return false;
            }

            // verificar si el rol del usuario es igual a "Administrador"
            return userViewModel.Role == (int)UserRol.Administrador;
            
        }
    }
}
