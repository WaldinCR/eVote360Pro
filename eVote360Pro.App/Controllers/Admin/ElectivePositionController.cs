using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.ElectivePosition;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eVote360Pro.App.Controllers.Admin
{
    public class ElectivePositionController : Controller
    {
        private readonly IElectivePositionService _positionService;

        public ElectivePositionController(IElectivePositionService positionService)
        {
            _positionService = positionService;
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _positionService.GetAllAsync();
            
            // Mapeo manual de DTO a ViewModel (para mantener las vistas limpias)
            var list = dtos.Select(d => new ElectivePositionViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                IsActive = d.IsActive
            }).ToList();

            return View(list);
        }

        public IActionResult Create()
        {
            return View("Save", new SaveElectivePositionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveElectivePositionViewModel vm)
        {
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
            var vm = await _positionService.GetByIdSaveViewModelAsync(id);
            if (vm == null) return RedirectToAction(nameof(Index));
            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveElectivePositionViewModel vm)
        {
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