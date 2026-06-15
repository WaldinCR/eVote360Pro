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
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContext;

        public PoliticalLeaderAssignmentController(
            IPoliticalLeaderAssignmentService assignmentService,
            IPoliticalPartyService partyService,
            IUserService userService,
            IHttpContextAccessor httpContext)
        {
            _assignmentService = assignmentService;
            _partyService = partyService;
            _userService = userService;
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
            var users = await _userService.GetAllAsync();
            foreach (var a in assignments)
            {
                var user = users.FirstOrDefault(u => u.Id == a.UserId);
                if (user != null)
                {
                    a.UserName = $"{user.Name} {user.LastName} ({user.UserName})";
                }
            }
            return View(assignments);
        }

        // GET: /Admin/PoliticalLeaderAssignment/Create
        public async Task<IActionResult> Create()
        {
            if (!IsAdmin()) return AccessDenied();

            var allAssignments = await _assignmentService.GetAllViewModel();
            await LoadCreateDropdownsAsync(allAssignments);

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
                var allAssignments = await _assignmentService.GetAllViewModel();
                await LoadCreateDropdownsAsync(allAssignments);
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
                var allAssignments = await _assignmentService.GetAllViewModel();
                await LoadCreateDropdownsAsync(allAssignments);
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

        private async Task LoadCreateDropdownsAsync(List<PoliticalLeaderAssignmentViewModel> allAssignments)
        {
            var allParties = await _partyService.GetAllViewModel();
            var assignedPartyIds = allAssignments.Select(a => a.PoliticalPartyId).ToList();
            ViewBag.AvailableParties = allParties
                .Where(p => p.IsActive && !assignedPartyIds.Contains(p.Id))
                .ToList();

            var allUsers = await _userService.GetAllAsync();
            var assignedUserIds = allAssignments.Select(a => a.UserId).ToList();
            ViewBag.AvailableLeaders = allUsers
                .Where(u => u.IsActive && u.Role == (int)eVote360Pro.Core.Domain.Common.Enums.UserRol.DirigentePolitico && !assignedUserIds.Contains(u.Id))
                .ToList();
        }
    }
}