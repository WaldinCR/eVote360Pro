using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.CandidatePosition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

namespace eVote360Pro.App.Controllers.Leader
{
    [Authorize(Roles = "Dirigente")]
    public class CandidatePositionController : Controller
    {
        private readonly ICandidatePositionService _candidatePositionService;
        private readonly IMapper _mapper;
        private readonly IUserSession _userSession;

        public CandidatePositionController(ICandidatePositionService candidatePositionService, IUserSession userSession, IMapper mapper)
        {
            _candidatePositionService = candidatePositionService;
            _mapper = mapper;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            int currentLeaderPartyId = 1; 

            var dtos = await _candidatePositionService.GetAllByPartyAsync(currentLeaderPartyId);
            
            var list = _mapper.Map<List<CandidatePositionViewModel>>(dtos);

            return View(list);
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

        private async Task LoadDropdownsAsync()
        {
            var positions = await _positionService.GetAllAsync();
            ViewBag.Positions = positions.Where(p => p.IsActive).ToList();

            ViewBag.Candidates = new List<object>();
        }
    }
}