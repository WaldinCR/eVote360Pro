using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Candidate;
using eVote360Pro.App.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.App.Areas.Leader.Controllers
{
    [Area("Leader")]
    public class CandidateController : Controller
    {
        private readonly ICandidateService _candidateService;
        private readonly IHttpContextAccessor _httpContext;

        public CandidateController(ICandidateService candidateService, IHttpContextAccessor httpContext)
        {
            _candidateService = candidateService;
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

        // GET: /Leader/Candidate
        public async Task<IActionResult> Index()
        {
            if (!IsLeader()) return AccessDenied();

            var candidates = await _candidateService.GetAllByPartyIdViewModel(GetPartyId());
            return View(candidates);
        }

        // GET: /Leader/Candidate/Create
        public IActionResult Create()
        {
            if (!IsLeader()) return AccessDenied();

            var vm = new SaveCandidateViewModel { PoliticalPartyId = GetPartyId() };
            return View(vm);
        }

        // POST: /Leader/Candidate/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveCandidateViewModel vm)
        {
            if (!IsLeader()) return AccessDenied();

            // Forzar siempre el partido del dirigente autenticado
            vm.PoliticalPartyId = GetPartyId();

            if (!ModelState.IsValid) return View(vm);

            if (vm.PhotoFile == null || vm.PhotoFile.Length == 0)
            {
                ModelState.AddModelError("PhotoFile", "La foto del candidato es requerida al crear.");
                return View(vm);
            }

            try
            {
                var savedVm = await _candidateService.AddAsync(vm);

                if (vm.PhotoFile != null && vm.PhotoFile.Length > 0)
                {
                    savedVm.PhotoUrl = UploadFile.Upload(vm.PhotoFile, savedVm.Id, "candidates");
                    await _candidateService.UpdateAsync(savedVm);
                }

                TempData["Success"] = "Candidato creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        // GET: /Leader/Candidate/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLeader()) return AccessDenied();

            var vm = await _candidateService.GetByIdSaveViewModel(id);
            if (vm == null) return NotFound();

            // Verificar que pertenece al partido del dirigente
            if (vm.PoliticalPartyId != GetPartyId()) return AccessDenied();

            return View(vm);
        }

        // POST: /Leader/Candidate/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SaveCandidateViewModel vm)
        {
            if (!IsLeader()) return AccessDenied();

            vm.PoliticalPartyId = GetPartyId();

            if (!ModelState.IsValid) return View(vm);

            try
            {
                var originalCandidate = await _candidateService.GetByIdSaveViewModel(vm.Id);
                if (originalCandidate != null && vm.PhotoFile != null && vm.PhotoFile.Length > 0)
                {
                    vm.PhotoUrl = UploadFile.Upload(vm.PhotoFile, vm.Id, "candidates", true, originalCandidate.PhotoUrl);
                }
                else if (originalCandidate != null)
                {
                    vm.PhotoUrl = originalCandidate.PhotoUrl;
                }

                await _candidateService.UpdateAsync(vm);
                TempData["Success"] = "Candidato actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        // POST: /Leader/Candidate/ChangeStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, bool currentStatus)
        {
            if (!IsLeader()) return AccessDenied();

            try
            {
                await _candidateService.ChangeStatusAsync(id, !currentStatus);
                TempData["Success"] = "Estado del candidato actualizado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}