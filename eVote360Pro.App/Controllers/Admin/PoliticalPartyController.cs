using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.PoliticalParty;
using eVote360Pro.Core.Application.Dtos.PoliticalParty;
using eVote360Pro.App.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace eVote360Pro.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PoliticalPartyController : Controller
    {
        private readonly IPoliticalPartyService _partyService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public PoliticalPartyController(IPoliticalPartyService partyService, IUserSession userSession, IMapper mapper)
        {
            _partyService = partyService;
            _userSession = userSession;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            var parties = await _partyService.GetAllAsync();
            var viewModels = parties.Select(p => new PoliticalPartyViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Acronym = p.Acronym,
                LogoUrl = p.LogoUrl,
                IsActive = p.IsActive
            }).ToList();
            return View(viewModels);
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
                var dto = _mapper.Map<SavePoliticalPartyDto>(vm);
                var savedDto = await _partyService.AddAsync(dto);

                if (savedDto != null && vm.LogoFile != null && vm.LogoFile.Length > 0)
                {
                    savedDto.LogoUrl = UploadFile.Upload(vm.LogoFile, savedDto.Id, "logos");
                    await _partyService.UpdateAsync(savedDto);
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

            var dto = await _partyService.GetByIdSaveDtoAsync(id);
            if (dto == null) return NotFound();
            var vm = _mapper.Map<SavePoliticalPartyViewModel>(dto);
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
                var originalPartyDto = await _partyService.GetByIdSaveDtoAsync(vm.Id);
                if (originalPartyDto != null && vm.LogoFile != null && vm.LogoFile.Length > 0)
                {
                    vm.LogoUrl = UploadFile.Upload(vm.LogoFile, vm.Id, "logos", true, originalPartyDto.LogoUrl);
                }
                else if (originalPartyDto != null)
                {
                    vm.LogoUrl = originalPartyDto.LogoUrl;
                }

                var dto = _mapper.Map<SavePoliticalPartyDto>(vm);
                await _partyService.UpdateAsync(dto);
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