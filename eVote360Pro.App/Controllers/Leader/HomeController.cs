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
        private readonly IUserSession _userSession;

        public HomeController(
            ICandidateService candidateService,
            IAllianceService allianceService,
            IAllianceRequestService requestService,
            IHttpContextAccessor httpContext,
            IUserSession userSession)
        {
            _candidateService = candidateService;
            _allianceService = allianceService;
            _requestService = requestService;
            _httpContext = httpContext;
            _userSession = userSession;
        }

        private int GetPartyId() =>
            _httpContext.HttpContext!.Session.GetInt32("PartyId") ?? 0;

        // GET: /Leader/Home
        public async Task<IActionResult> Index()
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsDirigente())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

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
