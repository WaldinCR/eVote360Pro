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

        public CandidatePositionController(ICandidatePositionService candidatePositionService, IMapper mapper)
        {
            _candidatePositionService = candidatePositionService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            // ID temporal fijo. Al integrar el Login, se reemplaza por la sesión del dirigente
            int currentLeaderPartyId = 1; 

            var dtos = await _candidatePositionService.GetAllByPartyAsync(currentLeaderPartyId);
            
            var list = _mapper.Map<List<CandidatePositionViewModel>>(dtos);

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create", new SaveCandidatePositionViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveCandidatePositionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", vm);
            }

            var error = await _candidatePositionService.AddAsync(vm);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return View("Create", vm);
            }

            TempData["Success"] = "Candidato asignado al puesto exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
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
    }
}