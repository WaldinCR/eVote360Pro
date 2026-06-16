using eVote360Pro.Core.Application.Dtos.User;
using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Login;
using eVote360Pro.Core.Application.ViewModels.User;
using eVote360Pro.Core.Domain.Common.Enums;
using eVote360Pro.Core.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.App.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserSession _userSession;
        private readonly IPoliticalLeaderAssignmentRepository _leaderAssignmentRepository;

        public LoginController(
            IUserService userService, 
            IUserSession userSession,
            IPoliticalLeaderAssignmentRepository leaderAssignmentRepository)
        {
            _userService = userService;
            _userSession = userSession;
            _leaderAssignmentRepository = leaderAssignmentRepository;
        }

        public IActionResult Index()
        {
            if (_userSession.HasUser()) 
            {
                UserViewModel? userSession = _userSession.GetUserSession();
                if (userSession != null)
                    return RedirectByRole(userSession.Role);
            }

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
            
            // Establecer valores adicionales en la sesión para vistas/layouts
            HttpContext.Session.SetString("UserRole", ((UserRol)user.Role).ToString());
            HttpContext.Session.SetString("UserName", $"{user.Name} {user.LastName}");

            if ((UserRol)user.Role == UserRol.DirigentePolitico)
            {
                var assignment = await _leaderAssignmentRepository.GetByUserIdAsync(user.Id);
                if (assignment != null)
                {
                    HttpContext.Session.SetInt32("PartyId", assignment.PoliticalPartyId);
                }
            }

            return RedirectByRole(user.Role);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToRoute(new { controller = "Login", action = "Index" });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectByRole(int role)
        {
            return (UserRol)role switch
            {
                UserRol.Administrador => RedirectToAction("Index", "Home", new { area = "Admin" }),
                UserRol.DirigentePolitico => RedirectToAction("Index", "Home", new { area = "Leader" }),
                _ => RedirectToAction("Index", "Login")
            };
        }
    }
}