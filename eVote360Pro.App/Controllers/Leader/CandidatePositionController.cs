using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.CandidatePosition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace eVote360Pro.App.Controllers.Leader
{
    // Bug #9 fix: Acceso exclusivo para DirigentePolitico
    [Authorize(Roles = "DirigentePolitico")]
    public class CandidatePositionController : Controller
    {
        private readonly ICandidatePositionService _assignmentService;
        private readonly IElectivePositionService _positionService;

        // Bug #10 fix: Se eliminó la dependencia directa de IGenericRepository<Candidate>.
        // La lógica de carga de candidatos disponibles ahora se resuelve a través del servicio.
        public CandidatePositionController(
            ICandidatePositionService assignmentService,
            IElectivePositionService positionService)
        {
            _assignmentService = assignmentService;
            _positionService = positionService;
        }

        public async Task<IActionResult> Index()
        {
            // Obtiene el ID del partido del dirigente en sesión (integración con módulo de Login del equipo)
            int partyId = HttpContext.Session.GetInt32("PartyId") ?? 1;

            var list = await _assignmentService.GetAllByPartyAsync(partyId);
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View(new SaveCandidatePositionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCandidatePositionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return View(vm);
            }

            // Asignamos el ID del partido en sesión al ViewModel
            vm.PoliticalPartyId = HttpContext.Session.GetInt32("PartyId") ?? 1;

            var error = await _assignmentService.AddAsync(vm);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                await LoadDropdownsAsync();
                return View(vm);
            }

            TempData["Success"] = "Candidato asignado al puesto exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var error = await _assignmentService.DeleteAsync(id);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
            }
            else
            {
                TempData["Success"] = "Asignación eliminada exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        // Método auxiliar para cargar los SelectList (Listas desplegables)
        // Solo puestos activos disponibles
        private async Task LoadDropdownsAsync()
        {
            var positions = await _positionService.GetAllAsync();
            ViewBag.Positions = positions.Where(p => p.IsActive).ToList();

            // Nota: La lista de candidatos disponibles para asignación (propios + aliados activos)
            // se cargará cuando el módulo de CandidateService esté integrado por el compañero.
            // Por ahora se usa una lista vacía para evitar dependencia directa de repositorios en el controlador.
            ViewBag.Candidates = new List<object>();
        }
    }
}