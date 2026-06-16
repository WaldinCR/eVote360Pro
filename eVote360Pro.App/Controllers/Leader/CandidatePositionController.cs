using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.CandidatePosition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVote360Pro.App.Controllers.Leader
{
    public class CandidatePositionController : Controller
    {
        private readonly ICandidatePositionService _candidatePositionService;
        private readonly ICandidateService _candidateService;
        private readonly IElectivePositionService _positionService;
        private readonly IAllianceService _allianceService;
        private readonly IUserSession _userSession;

        public CandidatePositionController(
            ICandidatePositionService candidatePositionService,
            ICandidateService candidateService,
            IElectivePositionService positionService,
            IAllianceService allianceService,
            IUserSession userSession)
        {
            _candidatePositionService = candidatePositionService;
            _candidateService = candidateService;
            _positionService = positionService;
            _allianceService = allianceService;
            _userSession = userSession;
        }

        private bool IsLeader() =>
            HttpContext.Session.GetString("UserRole") == "DirigentePolitico";

        private int GetPartyId() =>
            HttpContext.Session.GetInt32("PartyId") ?? 0;

        private IActionResult AccessDenied()
        {
            TempData["Error"] = "No tiene permisos para acceder a esta sección o debe iniciar sesión primero.";
            return RedirectToAction("Index", "Login", new { area = "" });
        }

        public async Task<IActionResult> Index()
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsDirigente())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });


            if (!IsLeader()) return AccessDenied();

            int partyId = GetPartyId();
            var dtos = await _candidatePositionService.GetAllByPartyAsync(partyId);
            
            var list = new List<CandidatePositionViewModel>();
            foreach (var dto in dtos)
            {
                list.Add(new CandidatePositionViewModel
                {
                    Id = dto.Id,
                    CandidateId = dto.CandidateId,
                    CandidateName = dto.CandidateName,
                    ElectivePositionId = dto.ElectivePositionId,
                    ElectivePositionName = dto.ElectivePositionName,
                    PoliticalPartyId = dto.PoliticalPartyId,
                    IsAllied = dto.IsAllied
                });
            }

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsDirigente())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });


            if (!IsLeader()) return AccessDenied();

            int partyId = GetPartyId();
            await LoadViewBagsAsync(partyId);

            return View("Create", new SaveCandidatePositionViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveCandidatePositionViewModel vm)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsDirigente())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });


            if (!IsLeader()) return AccessDenied();

            int partyId = GetPartyId();
            vm.PoliticalPartyId = partyId;

            if (!ModelState.IsValid)
            {
                await LoadViewBagsAsync(partyId);
                return View("Create", vm);
            }

            var error = await _candidatePositionService.AddAsync(vm);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                await LoadViewBagsAsync(partyId);
                return View("Create", vm);
            }

            TempData["Success"] = "Candidato asignado al puesto exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsDirigente())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            if (!IsLeader()) return AccessDenied();

            var error = await _candidatePositionService.DeleteAsync(id);
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

        private async Task LoadViewBagsAsync(int userPartyId)
        {
            var alliances = await _allianceService.GetAllViewModel();
            var alliedPartyIds = alliances
                .Where(a => a.Party1Id == userPartyId || a.Party2Id == userPartyId)
                .Select(a => a.Party1Id == userPartyId ? a.Party2Id : a.Party1Id)
                .ToList();

            var allCandidates = await _candidateService.GetAllViewModel();
            var availableCandidates = allCandidates
                .Where(c => c.IsActive && (c.PoliticalPartyId == userPartyId || alliedPartyIds.Contains(c.PoliticalPartyId)))
                .ToList();

            var allPositions = await _positionService.GetAllAsync();
            var availablePositions = allPositions
                .Where(p => p.IsActive)
                .ToList();

            ViewBag.Candidates = availableCandidates;
            ViewBag.Positions = availablePositions;
        }
    }
}