using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Alliance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.App.Areas.Leader.Controllers
{
    [Area("Leader")]
    public class AllianceController : Controller
    {
        private readonly IAllianceService _allianceService;
        private readonly IAllianceRequestService _requestService;
        private readonly IPoliticalPartyService _partyService;
        private readonly IHttpContextAccessor _httpContext;

        public AllianceController(
            IAllianceService allianceService,
            IAllianceRequestService requestService,
            IPoliticalPartyService partyService,
            IHttpContextAccessor httpContext)
        {
            _allianceService = allianceService;
            _requestService = requestService;
            _partyService = partyService;
            _httpContext = httpContext;
        }

        private bool IsLeader() =>
            _httpContext.HttpContext!.Session.GetString("UserRole") == "DirigentePolitico";

        private int GetPartyId() =>
            _httpContext.HttpContext!.Session.GetInt32("PartyId") ?? 0;

        private IActionResult AccessDenied()
        {
            TempData["Error"] = "No tiene permisos para acceder a esta sección.";
            return RedirectToAction("Index", "Home", new { area = "Leader" });
        }

        // GET: /Leader/Alliance
        // Muestra 3 secciones: pendientes recibidas, enviadas y alianzas vigentes
        public async Task<IActionResult> Index()
        {
            if (!IsLeader()) return AccessDenied();

            int partyId = GetPartyId();

            ViewBag.ReceivedPending = await _requestService.GetReceivedPendingAsync(partyId);
            ViewBag.SentRequests = await _requestService.GetSentByPartyAsync(partyId);
            ViewBag.ActiveAlliances = await _allianceService.GetAllViewModel();

            // Filtrar alianzas vigentes solo del partido del dirigente
            var allAlliances = (List<AllianceViewModel>)ViewBag.ActiveAlliances;
            ViewBag.ActiveAlliances = allAlliances
                .Where(a => a.Party1Id == partyId || a.Party2Id == partyId)
                .ToList();

            return View();
        }

        // GET: /Leader/Alliance/Create
        public async Task<IActionResult> Create()
        {
            if (!IsLeader()) return AccessDenied();

            int partyId = GetPartyId();

            // Partidos activos disponibles para alianza (excluir propio)
            var allParties = await _partyService.GetAllViewModel();
            ViewBag.AvailableParties = allParties
                .Where(p => p.IsActive && p.Id != partyId)
                .ToList();

            return View(new CreateAllianceRequestViewModel { ApplicantPartyId = partyId });
        }

        // POST: /Leader/Alliance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAllianceRequestViewModel vm)
        {
            if (!IsLeader()) return AccessDenied();

            vm.ApplicantPartyId = GetPartyId();

            if (!ModelState.IsValid)
            {
                var allParties = await _partyService.GetAllViewModel();
                ViewBag.AvailableParties = allParties
                    .Where(p => p.IsActive && p.Id != vm.ApplicantPartyId)
                    .ToList();
                return View(vm);
            }

            try
            {
                await _requestService.AddAsync(vm);
                TempData["Success"] = "Solicitud de alianza enviada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var allParties = await _partyService.GetAllViewModel();
                ViewBag.AvailableParties = allParties
                    .Where(p => p.IsActive && p.Id != vm.ApplicantPartyId)
                    .ToList();
                return View(vm);
            }
        }

        // POST: /Leader/Alliance/Accept/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int id)
        {
            if (!IsLeader()) return AccessDenied();

            try
            {
                await _requestService.AcceptRequestAsync(id);
                TempData["Success"] = "Solicitud de alianza aceptada. Alianza vigente creada.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Leader/Alliance/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            if (!IsLeader()) return AccessDenied();

            try
            {
                await _requestService.RejectRequestAsync(id);
                TempData["Success"] = "Solicitud de alianza rechazada.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Leader/Alliance/DeleteRequest/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            if (!IsLeader()) return AccessDenied();

            try
            {
                await _requestService.DeleteAsync(id, GetPartyId());
                TempData["Success"] = "Solicitud eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Leader/Alliance/DeleteAlliance/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAlliance(int id)
        {
            if (!IsLeader()) return AccessDenied();

            try
            {
                await _allianceService.DeleteAsync(id);
                TempData["Success"] = "Alianza eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}