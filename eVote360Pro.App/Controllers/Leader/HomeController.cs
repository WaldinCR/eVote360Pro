using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Leader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eVote360Pro.App.Areas.Leader.Controllers
{
    [Area("Leader")]
    public class HomeController : Controller
    {
        private readonly ICandidateService _candidateService;
        private readonly IAllianceService _allianceService;
        private readonly IAllianceRequestService _requestService;
        private readonly IHttpContextAccessor _httpContext;

        public HomeController(
            ICandidateService candidateService,
            IAllianceService allianceService,
            IAllianceRequestService requestService,
            IHttpContextAccessor httpContext)
        {
            _candidateService = candidateService;
            _allianceService = allianceService;
            _requestService = requestService;
            _httpContext = httpContext;
        }

        private bool IsLeader() =>
            _httpContext.HttpContext!.Session.GetString("UserRole") == "DirigentePolitico";

        private int GetPartyId() =>
            _httpContext.HttpContext!.Session.GetInt32("PartyId") ?? 0;

        // GET: /Leader/Home
        public async Task<IActionResult> Index()
        {
            if (!IsLeader())
            {
                TempData["Error"] = "Acceso denegado.";
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            int partyId = GetPartyId();

            var candidates = await _candidateService.GetAllByPartyIdViewModel(partyId);
            var alliances = await _allianceService.GetAllViewModel();
            var receivedPending = await _requestService.GetReceivedPendingAsync(partyId);

            var vm = new LeaderHomeViewModel
            {
                TotalCandidates = candidates.Count,
                ActiveCandidates = candidates.Count(c => c.IsActive),
                InactiveCandidates = candidates.Count(c => !c.IsActive),
                ActiveAlliances = alliances.Count(a => a.Party1Id == partyId || a.Party2Id == partyId),
                PendingAllianceRequests = receivedPending.Count
            };

            return View(vm);
        }
    }
}
