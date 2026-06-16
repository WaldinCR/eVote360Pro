using AutoMapper;
using eVote360Pro.Core.Application.Dtos.User;
using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.User;
using eVote360Pro.Core.Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserSession _userSession;
        private readonly IElectionService _electionService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IUserSession userSession, IMapper mapper, IElectionService electionService)
        {
            _userService = userService;
            _userSession = userSession;
            _mapper = mapper;
            _electionService = electionService;
        }

        public async Task<IActionResult> Index()
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            var users = await _userService.GetAllAsync();
            var viewModels = _mapper.Map<List<UserViewModel>>(users);
            
            var elections = await _electionService.GetAllAsync();
            ViewBag.HasActiveElection = elections.Any(e => e.Status == ElectionStatus.Active);

            return View(viewModels);
        }

        public async Task<IActionResult> Create()
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            // bloquear si hay elección activa
            var elections = await _electionService.GetAllAsync();
            bool hasActiveElection = elections.Any(e => e.Status == ElectionStatus.Active);

            if (hasActiveElection)
            {
                TempData["Error"] = "No se puede crear un usuario mientras exista una elección activa.";
                return RedirectToRoute(new { area = "Admin", controller = "User", action = "Index" });
            }
            return View(new SaveUserViewModel
            {
                Name = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                UserName = string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
                Role = 0
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveUserViewModel vm)
        {

            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            if (await HasActiveElection())
            {
                TempData["Error"] =
                    "No se puede crear un usuario mientras exista una elección activa.";

                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid) return View(vm);

            if (await _userService.ExistsUserNameAsync(vm.UserName.Trim()))
            {
                ModelState.AddModelError("UserName", "Ya existe un usuario registrado con este nombre de usuario.");
                return View(vm);
            }

            if (await _userService.ExistsEmailAsync(vm.Email.Trim()))
            {
                ModelState.AddModelError("Email", "Ya existe un usuario registrado con este correo electrónico.");
                return View(vm);
            }

            var dto = _mapper.Map<SaveUserDto>(vm);
            await _userService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            if (await HasActiveElection())
            {
                TempData["Error"] =
                    "No se puede modificar un usuario mientras exista una elección activa.";

                return RedirectToAction(nameof(Index));
            }

            var dto = await _userService.GetByIdSaveDtoAsync(id);
            if (dto == null)
                return RedirectToRoute(new { area = "Admin", controller = "User", action = "Index" });

            var vm = _mapper.Map<SaveUserViewModel>(dto);
            vm.Password = string.Empty;
            vm.ConfirmPassword = string.Empty;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserViewModel vm)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            if (await HasActiveElection())
            {
                TempData["Error"] =
                    "No se puede modificar un usuario mientras exista una elección activa.";

                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrEmpty(vm.Password))
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

            if (!ModelState.IsValid) return View(vm);

            // Validar username unico excluyendo el usuario actual
            var currentDto = await _userService.GetByIdSaveDtoAsync(vm.Id);
            if (currentDto == null)
                return RedirectToRoute(new { area = "Admin", controller = "User", action = "Index" });

            if (currentDto.UserName.Trim() != vm.UserName.Trim() &&
                await _userService.ExistsUserNameAsync(vm.UserName.Trim()))
            {
                ModelState.AddModelError("UserName", "Ya existe un usuario registrado con este nombre de usuario.");
                return View(vm);
            }

            if (currentDto.Email.Trim() != vm.Email.Trim() &&
                await _userService.ExistsEmailAsync(vm.Email.Trim()))
            {
                ModelState.AddModelError("Email", "Ya existe un usuario registrado con este correo electrónico.");
                return View(vm);
            }

            // No puede modificar su propio rol
            var sessionUser = HttpContext.Session.Get<UserViewModel>("User");
            if (sessionUser?.Id == vm.Id && currentDto.Role != vm.Role)
            {
                ModelState.AddModelError("", "No puede cambiar su propio rol ni desactivar su propio usuario mientras está autenticado.");
                return View(vm);
            }

            var dto = _mapper.Map<SaveUserDto>(vm);
            await _userService.UpdateAsync(dto);
            return RedirectToRoute(new { area = "Admin", controller = "User", action = "Index" });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            if (await HasActiveElection())
            {
                TempData["Error"] =
                    "No se puede modificar el estado de un usuario mientras exista una elección activa.";

                return RedirectToAction(nameof(Index));
            }

            var sessionUser = HttpContext.Session.Get<UserViewModel>("User");

            // No puede desactivarse a sí mismo
            if (sessionUser?.Id == id)
            {
                TempData["Error"] = "No puede cambiar su propio rol ni desactivar su propio usuario mientras está autenticado.";
                return RedirectToAction(nameof(Index));
            }

            var errorMsg = await _userService.ToggleActiveStatusAsync(id);
            if (errorMsg != null)
                TempData["Error"] = errorMsg;

            return RedirectToAction(nameof(Index));
        }
        private async Task<bool> HasActiveElection()
        {
            var elections = await _electionService.GetAllAsync();
            return elections.Any(e => e.Status == ElectionStatus.Active);
        }
    }
}