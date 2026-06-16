using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IPoliticalPartyService _partyService;
        private readonly ICandidateService _candidateService;
        private readonly IElectionService _electionService;
        private readonly IUserSession _userSession;

        public HomeController(
            IPoliticalPartyService partyService,
            ICandidateService candidateService,
            IElectionService electionService,
            IUserSession userSession)
        {
            _partyService = partyService;
            _candidateService = candidateService;
            _electionService = electionService;
            _userSession = userSession;
        }

        // GET: /Admin/Home
        public async Task<IActionResult> Index(int? year)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            var parties = await _partyService.GetAllViewModel();
            var candidates = await _candidateService.GetAllViewModel();
            var electionsList = await _electionService.GetAllAsync();

            var availableYears = electionsList.Select(e => e.Year).Distinct().OrderByDescending(y => y).ToList();
            int selectedYear = year ?? (availableYears.Any() ? availableYears.First() : DateTime.Now.Year);

            var summaries = await _electionService.GetElectionSummariesByYearAsync(selectedYear);

            var vm = new AdminHomeViewModel
            {
                SelectedYear = selectedYear,
                AvailableYears = availableYears,
                Elections = summaries,
                TotalParties = parties.Count,
                ActiveParties = parties.Count(p => p.IsActive),
                InactiveParties = parties.Count(p => !p.IsActive),
                TotalCandidates = candidates.Count,
                ActiveCandidates = candidates.Count(c => c.IsActive),
            };

            return View(vm);
        }
    }
}