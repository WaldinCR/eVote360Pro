using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Election;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace eVote360Pro.App.Controllers.Admin
{
    [Authorize(Roles = "Administrador")]
    public class ElectionController : Controller
    {
        private readonly IElectionService _electionService;

        public ElectionController(IElectionService electionService)
        {
            _electionService = electionService;
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _electionService.GetAllAsync();
            
            var list = dtos.Select(d => new ElectionViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Year = d.Year,
                Status = d.Status
            }).ToList();

            return View(list);
        }

        public IActionResult Create()
        {
            return View("Save", new SaveElectionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveElectionViewModel vm)
        {
            if (!ModelState.IsValid) return View("Save", vm);

            var error = await _electionService.AddAsync(vm);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return View("Save", vm);
            }

            TempData["Success"] = "Elección registrada exitosamente. Se encuentra en estado Pendiente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Activate(int id)
        {
            var error = await _electionService.ActivateElectionAsync(id);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error; // Aquí se mostrará si falta configurar un candidato o puesto
            }
            else
            {
                TempData["Success"] = "¡La elección ha sido activada! Los votantes ya pueden acceder al sistema.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Finish(int id)
        {
            var error = await _electionService.FinishElectionAsync(id);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
            }
            else
            {
                TempData["Success"] = "La elección ha finalizado. Ahora puede visualizar los resultados.";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Results(int id)
        {
            var results = await _electionService.GetResultsAsync(id);
            if (results == null)
            {
                TempData["Error"] = "La elección no existe o aún no ha finalizado.";
                return RedirectToAction(nameof(Index));
            }

            return View(results);
        }
    }
}