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
        private readonly IUserSession _userSession;

        public CandidateController(ICandidateService candidateService, IHttpContextAccessor httpContext, IUserSession userSession)
        {
            _candidateService = candidateService;
            _httpContext = httpContext;
            _userSession = userSession;
        }

        private int GetPartyId() =>
            _httpContext.HttpContext!.Session.GetInt32("PartyId") ?? 0;

        // GET: /Leader/Candidate
        public async Task<IActionResult> Index()
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsDirigente())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            var candidates = await _candidateService.GetAllByPartyIdViewModel(GetPartyId());
            return View(candidates);
        }

        // GET: /Leader/Candidate/Create
        public IActionResult Create()
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsDirigente())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            var vm = new SaveCandidateViewModel { PoliticalPartyId = GetPartyId() };
            return View(vm);
        }

        // POST: /Leader/Candidate/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveCandidateViewModel vm)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsDirigente())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

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

                if (savedVm != null && vm.PhotoFile != null && vm.PhotoFile.Length > 0)
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
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsDirigente())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            var vm = await _candidateService.GetByIdSaveViewModel(id);
            if (vm == null) return NotFound();

            // Verificar que pertenece al partido del dirigente
            if (vm.PoliticalPartyId != GetPartyId())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            return View(vm);
        }

        // POST: /Leader/Candidate/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SaveCandidateViewModel vm)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsDirigente())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            vm.PoliticalPartyId = GetPartyId();

            if (!ModelState.IsValid) return View(vm);

            try
            {
                var originalCandidate = await _candidateService.GetByIdSaveViewModel(vm.Id);
                if (originalCandidate == null || originalCandidate.PoliticalPartyId != GetPartyId())
                {
                    return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });
                }

                if (vm.PhotoFile != null && vm.PhotoFile.Length > 0)
                {
                    vm.PhotoUrl = UploadFile.Upload(vm.PhotoFile, vm.Id, "candidates", true, originalCandidate.PhotoUrl);
                }
                else
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
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsDirigente())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            try
            {
                var candidate = await _candidateService.GetByIdSaveViewModel(id);
                if (candidate == null || candidate.PoliticalPartyId != GetPartyId())
                {
                    return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });
                }

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