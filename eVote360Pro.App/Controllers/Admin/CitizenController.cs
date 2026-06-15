using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Citizen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace eVote360Pro.App.Controllers.Admin
{
    [Authorize(Roles = "Administrador")]
    public class CitizenController : Controller
    {
        private readonly ICitizenService _citizenService;
        private readonly IMapper _mapper;

        public CitizenController(ICitizenService citizenService, IMapper mapper)
        {
            _citizenService = citizenService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.IsElectionActive = await _citizenService.IsElectionActiveAsync(); 
            var dtos = await _citizenService.GetAllAsync();

            var list = _mapper.Map<List<CitizenViewModel>>(dtos);

            return View(list);
        }
        

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (await _citizenService.IsElectionActiveAsync())
            {
                TempData["Error"] = "No se puede acceder al formulario de creación mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            
            return View("Save", new SaveCitizenViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveCitizenViewModel vm)
        {
            if (await _citizenService.IsElectionActiveAsync())
            {
                TempData["Error"] = "No se puede crear un ciudadano mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View("Save", vm);
            }

            var error = await _citizenService.AddAsync(vm);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return View("Save", vm);
            }

            TempData["Success"] = "Ciudadano registrado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (await _citizenService.IsElectionActiveAsync())
            {
                TempData["Error"] = "No se pueden modificar ciudadanos mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

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

        [HttpGet]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            if (await _citizenService.IsElectionActiveAsync())
            {
                TempData["Error"] = "Acción bloqueada: Existe una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var vm = await _citizenService.GetByIdSaveViewModelAsync(id);
            if (vm == null) return RedirectToAction(nameof(Index));

            return View("ConfirmToggleStatus", vm);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatusPost(int id)
        {
            var error = await _citizenService.DeleteLogicalAsync(id);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
            }
            else
            {
                TempData["Success"] = "Estado del ciudadano modificado exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}