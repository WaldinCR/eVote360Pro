using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.PoliticalParty;
using eVote360Pro.App.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PoliticalPartyController : Controller
    {
        private readonly IPoliticalPartyService _partyService;
        private readonly IHttpContextAccessor _httpContext;

        public PoliticalPartyController(IPoliticalPartyService partyService, IHttpContextAccessor httpContext)
        {
            _partyService = partyService;
            _httpContext = httpContext;
        }

        // Solo administradores
        private bool IsAdmin() =>
            _httpContext.HttpContext!.Session.GetString("UserRole") == "Administrador";

        private IActionResult AccessDenied()
        {
            TempData["Error"] = "No tiene permisos para acceder a esta sección.";
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        // GET: /Admin/PoliticalParty
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) return AccessDenied();

            var parties = await _partyService.GetAllViewModel();
            return View(parties);
        }

        // GET: /Admin/PoliticalParty/Create
        public IActionResult Create()
        {
            if (!IsAdmin()) return AccessDenied();
            return View(new SavePoliticalPartyViewModel());
        }

        // POST: /Admin/PoliticalParty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SavePoliticalPartyViewModel vm)
        {
            if (!IsAdmin()) return AccessDenied();

            if (!ModelState.IsValid) return View(vm);

            if (vm.LogoFile == null || vm.LogoFile.Length == 0)
            {
                ModelState.AddModelError("LogoFile", "El logo del partido es requerido al crear.");
                return View(vm);
            }

            try
            {
                var savedVm = await _partyService.AddAsync(vm);

                if (vm.LogoFile != null && vm.LogoFile.Length > 0)
                {
                    savedVm.LogoUrl = UploadFile.Upload(vm.LogoFile, savedVm.Id, "logos");
                    await _partyService.UpdateAsync(savedVm);
                }

                TempData["Success"] = "Partido político creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        // GET: /Admin/PoliticalParty/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin()) return AccessDenied();

            var vm = await _partyService.GetByIdSaveViewModel(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        // POST: /Admin/PoliticalParty/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SavePoliticalPartyViewModel vm)
        {
            if (!IsAdmin()) return AccessDenied();

            if (!ModelState.IsValid) return View(vm);

            try
            {
                var originalParty = await _partyService.GetByIdSaveViewModel(vm.Id);
                if (originalParty != null && vm.LogoFile != null && vm.LogoFile.Length > 0)
                {
                    vm.LogoUrl = UploadFile.Upload(vm.LogoFile, vm.Id, "logos", true, originalParty.LogoUrl);
                }
                else if (originalParty != null)
                {
                    vm.LogoUrl = originalParty.LogoUrl;
                }

                await _partyService.UpdateAsync(vm);
                TempData["Success"] = "Partido político actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        // POST: /Admin/PoliticalParty/ChangeStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            if (!IsAdmin()) return AccessDenied();

            try
            {
                await _partyService.ChangeStatusAsync(id);
                TempData["Success"] = "Estado del partido actualizado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}