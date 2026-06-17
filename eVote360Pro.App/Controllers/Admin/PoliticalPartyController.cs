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
        private readonly IUserSession _userSession;

        public PoliticalPartyController(IPoliticalPartyService partyService, IUserSession userSession)
        {
            _partyService = partyService;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            var parties = await _partyService.GetAllViewModel();
            return View(parties);
        }

        public IActionResult Create()
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });
            return View(new SavePoliticalPartyViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePoliticalPartyViewModel vm)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            if (!ModelState.IsValid) return View(vm);

            if (vm.LogoFile == null || vm.LogoFile.Length == 0)
            {
                ModelState.AddModelError("LogoFile", "El logo del partido es requerido al crear.");
                return View(vm);
            }

            try
            {
                var savedVm = await _partyService.AddAsync(vm);

                if (savedVm != null && vm.LogoFile != null && vm.LogoFile.Length > 0)
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

        public async Task<IActionResult> Edit(int id)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            var vm = await _partyService.GetByIdSaveViewModel(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePoliticalPartyViewModel vm)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

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

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

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