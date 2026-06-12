using eVote360Pro.Core.Application.Dtos.User;
using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Login;
using eVote360Pro.Core.Application.ViewModels.User;
using eVote360Pro.Core.Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.App.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var sessionUser = HttpContext.Session.Get<UserViewModel>("User");
            if (sessionUser != null)
                return RedirectByRole(sessionUser.Role);

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userService.LoginAsync(new LoginDto
            {
                UserName = vm.UserName,
                Password = vm.Password
            });

            if (user == null)
            {
                ModelState.AddModelError("", "Los datos de acceso son inválidos.");
                return View(vm);
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "El usuario está inactivo.");
                return View(vm);
            }

            // Validar que dirigente tenga partido asignado (lo verifica el servicio)
            if ((UserRol)user.Role == UserRol.DirigentePolitico)
            {
                bool hasParty = await _userService.HasPoliticalPartyAssignedAsync(user.Id);
                if (!hasParty)
                {
                    ModelState.AddModelError("", "No tiene un partido político asignado, por lo tanto no puede iniciar sesión. Por favor, póngase en contacto con un administrador.");
                    return View(vm);
                }
            }

            var sessionUser = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                Role = user.Role,
                IsActive = user.IsActive
            };

            HttpContext.Session.Set("User", sessionUser);
            return RedirectByRole(user.Role);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectByRole(int role)
        {
            return (UserRol)role switch
            {
                UserRol.Administrador => RedirectToAction("Index", "Usuario", new { area = "Admin" }),
                UserRol.DirigentePolitico => RedirectToAction("Index", "Home", new { area = "Dirigente" }),
                _ => RedirectToAction("Index", "Login")
            };
        }
    }
}