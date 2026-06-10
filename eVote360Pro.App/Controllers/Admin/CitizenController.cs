using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Citizen;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace eVote360Pro.App.Controllers.Admin
{
    public class CitizenController : Controller
    {
        private readonly ICitizenService _citizenService;

        public CitizenController(ICitizenService citizenService)
        {
            _citizenService = citizenService;
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _citizenService.GetAllAsync();
            var list = dtos.Select(d => new CitizenViewModel
            {
                Id = d.Id,
                Document = d.Document,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email,
                IsActive = d.IsActive
            }).ToList();

            return View(list);
        }

        public IActionResult Create()
        {
            return View("Save", new SaveCitizenViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCitizenViewModel vm)
        {
            if (!ModelState.IsValid) return View("Save", vm);

            var error = await _citizenService.AddAsync(vm);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return View("Save", vm);
            }

            TempData["Success"] = "Ciudadano registrado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _citizenService.GetByIdSaveViewModelAsync(id);
            if (vm == null) return RedirectToAction(nameof(Index));
            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveCitizenViewModel vm)
        {
            if (!ModelState.IsValid) return View("Save", vm);

            var error = await _citizenService.UpdateAsync(vm);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return View("Save", vm);
            }

            TempData["Success"] = "Ciudadano actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var error = await _citizenService.DeleteLogicalAsync(id);
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