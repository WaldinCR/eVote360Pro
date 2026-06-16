using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.ElectivePosition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace eVote360Pro.App.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador")]
    public class ElectivePositionController : Controller
    {
        private readonly IElectivePositionService _positionService;
        private readonly IMapper _mapper;
        private readonly IUserSession _userSession;

        public ElectivePositionController(IElectivePositionService positionService, IMapper mapper, IUserSession userSession)
        {
            _positionService = positionService;
            _mapper = mapper;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            var dtos = await _positionService.GetAllAsync();
            
            var list = _mapper.Map<List<ElectivePositionViewModel>>(dtos);

            return View(list);
        }

        public IActionResult Create()
        {
            return View("Save", new SaveElectivePositionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveElectivePositionViewModel vm)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            
            if (!ModelState.IsValid) return View("Save", vm);

            var error = await _positionService.AddAsync(vm);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return View("Save", vm);
            }

            TempData["Success"] = "Puesto electivo creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            var vm = await _positionService.GetByIdSaveViewModelAsync(id);
            if (vm == null) return RedirectToAction(nameof(Index));
            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveElectivePositionViewModel vm)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });


            if (!ModelState.IsValid) return View("Save", vm);

            var error = await _positionService.UpdateAsync(vm);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return View("Save", vm);
            }

            TempData["Success"] = "Puesto electivo actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

                
            var error = await _positionService.DeleteLogicalAsync(id);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
            }
            else
            {
                TempData["Success"] = "Estado modificado exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}