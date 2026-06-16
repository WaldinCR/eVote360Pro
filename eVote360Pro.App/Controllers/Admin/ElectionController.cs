using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Election;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace eVote360Pro.App.Controllers.Admin
{
    [Authorize(Roles = "Administrador")]
    public class ElectionController : Controller
    {
        private readonly IElectionService _electionService;
        private readonly IMapper _mapper;
        private readonly IUserSession _userSession;

        public ElectionController(IElectionService electionService, IMapper mapper, IUserSession userSession)
        {
            _electionService = electionService;
            _mapper = mapper;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });
            var dtos = await _electionService.GetAllAsync();
            
            var list = _mapper.Map<List<ElectionViewModel>>(dtos);

            return View(list);
        }

        public IActionResult Create()
        {
            return View("Save", new SaveElectionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveElectionViewModel vm)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });


            if (!ModelState.IsValid) return View("Save", vm);

            var error = await _electionService.AddAsync(vm);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
                return View("Save", vm);
            }

            TempData["Success"] = "Elección registrada exitosamente. Se encuentra en estado Pendiente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Activate(int id)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });


            var error = await _electionService.ActivateElectionAsync(id);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error; 
            }
            else
            {
                TempData["Success"] = "¡La elección ha sido activada! Los votantes ya pueden acceder al sistema.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Finish(int id)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });


            var error = await _electionService.FinishElectionAsync(id);
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
            }
            else
            {
                TempData["Success"] = "La elección ha finalizado. Ahora puede visualizar los resultados.";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Results(int id)
        {
            if (!_userSession.HasUser())
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            if (!_userSession.IsAdmin())
                return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

                
            var results = await _electionService.GetResultsAsync(id);
            if (results == null)
            {
                TempData["Error"] = "La elección no existe o aún no ha finalizado.";
                return RedirectToAction(nameof(Index));
            }

            return View(results);
        }
    }
}