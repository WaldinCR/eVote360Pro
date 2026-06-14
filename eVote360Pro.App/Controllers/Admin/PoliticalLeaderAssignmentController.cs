using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.PoliticalLeaderAssignment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PoliticalLeaderAssignmentController : Controller
    {
        private readonly IPoliticalLeaderAssignmentService _assignmentService;
        private readonly IPoliticalPartyService _partyService;
        private readonly IHttpContextAccessor _httpContext;

        public PoliticalLeaderAssignmentController(
            IPoliticalLeaderAssignmentService assignmentService,
            IPoliticalPartyService partyService,
            IHttpContextAccessor httpContext)
        {
            _assignmentService = assignmentService;
            _partyService = partyService;
            _httpContext = httpContext;
        }

        private bool IsAdmin() =>
            _httpContext.HttpContext!.Session.GetString("UserRole") == "Administrador";

        private IActionResult AccessDenied()
        {
            TempData["Error"] = "No tiene permisos para acceder a esta sección.";
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        // GET: /Admin/PoliticalLeaderAssignment
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) return AccessDenied();

            var assignments = await _assignmentService.GetAllViewModel();
            return View(assignments);
        }

        // GET: /Admin/PoliticalLeaderAssignment/Create
        public async Task<IActionResult> Create()
        {
            if (!IsAdmin()) return AccessDenied();

            // Cargar partidos activos disponibles (sin dirigente asignado)
            var allParties = await _partyService.GetAllViewModel();
            var allAssignments = await _assignmentService.GetAllViewModel();
            var assignedPartyIds = allAssignments.Select(a => a.PoliticalPartyId).ToList();

            ViewBag.AvailableParties = allParties
                .Where(p => p.IsActive && !assignedPartyIds.Contains(p.Id))
                .ToList();

            return View(new SavePoliticalLeaderAssignmentViewModel());
        }

        // POST: /Admin/PoliticalLeaderAssignment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SavePoliticalLeaderAssignmentViewModel vm)
        {
            if (!IsAdmin()) return AccessDenied();

            if (!ModelState.IsValid)
            {
                var allParties = await _partyService.GetAllViewModel();
                var allAssignments = await _assignmentService.GetAllViewModel();
                var assignedPartyIds = allAssignments.Select(a => a.PoliticalPartyId).ToList();
                ViewBag.AvailableParties = allParties
                    .Where(p => p.IsActive && !assignedPartyIds.Contains(p.Id))
                    .ToList();
                return View(vm);
            }

            try
            {
                await _assignmentService.AddAsync(vm);
                TempData["Success"] = "Dirigente asignado correctamente al partido.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var allParties = await _partyService.GetAllViewModel();
                var allAssignments = await _assignmentService.GetAllViewModel();
                var assignedPartyIds = allAssignments.Select(a => a.PoliticalPartyId).ToList();
                ViewBag.AvailableParties = allParties
                    .Where(p => p.IsActive && !assignedPartyIds.Contains(p.Id))
                    .ToList();
                return View(vm);
            }
        }

        // POST: /Admin/PoliticalLeaderAssignment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin()) return AccessDenied();

            try
            {
                await _assignmentService.DeleteAsync(id);
                TempData["Success"] = "Asignación eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}