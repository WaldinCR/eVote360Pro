using eVote360Pro.Core.Application.Dtos.Vote;
using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Elector;
using eVote360Pro.Core.Domain.Common.Enums;
using eVote360Pro.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.App.Areas.Elector.Controllers
{
    [Area("Elector")]
    public class ElectorController : Controller
    {
        private readonly IElectorService _electorService;
        private readonly IOcrService _ocrService;
        private readonly IElectionService _electionService;
        private readonly ICitizenService _citizenService;
        private readonly ICitizenVoteRepository _citizenVoteRepository;
        private readonly IElectivePositionService _electivePositionService;

        public ElectorController(
            IElectorService electorService,
            IOcrService ocrService,
            IElectionService electionService,
            ICitizenService citizenService,
            ICitizenVoteRepository citizenVoteRepository,
            IElectivePositionService electivePositionService)
        {
            _electorService = electorService;
            _ocrService = ocrService;
            _electionService = electionService;
            _citizenService = citizenService;
            _citizenVoteRepository = citizenVoteRepository;
            _electivePositionService = electivePositionService;
        }

        // 1 Ingresar documento
        public async Task<IActionResult> Index()
        {
            var elections = await _electionService.GetAllAsync();
            bool hasActiveElection = elections.Any(e => e.Status == ElectionStatus.Active);

            if (!hasActiveElection)
                ViewBag.Message = "No hay ningún proceso electoral en estos momentos.";

            return View(new AddDocumentViewModel { Document = string.Empty });
        }

        //  2 Validar documento
        [HttpPost]
        public async Task<IActionResult> Index(AddDocumentViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // Verificar elección activa
            var elections = await _electionService.GetAllAsync();
            var activeElection = elections.FirstOrDefault(e => e.Status == ElectionStatus.Active);

            if (activeElection == null)
            {
                ModelState.AddModelError("", "No hay ningún proceso electoral en estos momentos.");
                return View(vm);
            }

            // Verificar ciudadano
            var citizens = await _citizenService.GetAllAsync();
            var citizen = citizens.FirstOrDefault(c => c.Document == vm.Document.Trim());

            if (citizen == null)
            {
                ModelState.AddModelError("Document", "No existe un ciudadano registrado con este número de documento.");
                return View(vm);
            }

            if (!citizen.IsActive)
            {
                ModelState.AddModelError("Document", "Este ciudadano se encuentra inactivo y no puede participar en el proceso de votación.");
                return View(vm);
            }

            // Verificar si ya votó
            bool alreadyVoted = await _citizenVoteRepository.HasAlreadyVotedAsync(citizen.Id, activeElection.Id);
            if (alreadyVoted)
            {
                ModelState.AddModelError("Document", "Ya ha ejercido su derecho al voto.");
                return View(vm);
            }

            // Verificar que tiene email
            if (string.IsNullOrWhiteSpace(citizen.Email))
            {
                ModelState.AddModelError("", "Este ciudadano no tiene un correo electrónico registrado. No es posible continuar con la verificación de identidad.");
                return View(vm);
            }

            // Guardar en sesión
            HttpContext.Session.Set("DocumentoIngresado", vm.Document.Trim());
            HttpContext.Session.Set("ElectionId", activeElection.Id);
            HttpContext.Session.Set("CitizenId", citizen.Id);
            HttpContext.Session.Set("CitizenName", $"{citizen.FirstName} {citizen.LastName}");
            HttpContext.Session.Set("CitizenEmail", citizen.Email);

            return RedirectToAction(nameof(LoadCedula));
        }

        // 3 Cargar cédula
        public IActionResult LoadCedula()
        {
            var document = HttpContext.Session.Get<string>("DocumentoIngresado");
            if (string.IsNullOrEmpty(document))
                return RedirectToAction(nameof(Index));

            return View(new LoadCedulaViewModel { Document = document, CedulaImage = null });
        }

        // 3 Procesar OCR
        [HttpPost]
        public async Task<IActionResult> LoadCedula(LoadCedulaViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (!_ocrService.IsValidImageFormat(vm.CedulaImage!))
            {
                ModelState.AddModelError("CedulaImage", "El archivo seleccionado no tiene un formato de imagen válido.");
                return View(vm);
            }

            var expectedDocument = HttpContext.Session.Get<string>("DocumentoIngresado");
            var citizenId = HttpContext.Session.Get<int>("CitizenId");
            var electionId = HttpContext.Session.Get<int>("ElectionId");
            var citizenName = HttpContext.Session.Get<string>("CitizenName");
            var email = HttpContext.Session.Get<string>("CitizenEmail");

            var extractedText = await _ocrService.ExtractDocumentNumberAsync(vm.CedulaImage!);

            if (string.IsNullOrWhiteSpace(extractedText))
            {
                ModelState.AddModelError("CedulaImage", "No fue posible leer correctamente el número de documento en la imagen cargada. Por favor, suba una imagen más clara.");
                return View(vm);
            }

            var normalizedExtracted = extractedText.Replace("-", "").Replace(" ", "").Trim();
            var normalizedExpected = expectedDocument!.Replace("-", "").Replace(" ", "").Trim();

            if (!normalizedExtracted.Contains(normalizedExpected))
            {
                ModelState.AddModelError("CedulaImage", "Los datos extraídos de la foto no coinciden con los datos previamente ingresados por el elector.");
                return View(vm);
            }

            bool sent = await _electorService.GenerateAndSendOtpAsync(citizenId, electionId, citizenName!, email!);
            if (!sent)
            {
                ModelState.AddModelError("", "No fue posible enviar el código de verificación. Intente nuevamente más tarde.");
                return View(vm);
            }

            HttpContext.Session.Set("OcrValidated", true);
            return RedirectToAction(nameof(ValidateOtp));
        }

        // 4 Validar OTP
        public IActionResult ValidateOtp()
        {
            if (!HttpContext.Session.Get<bool>("OcrValidated"))
                return RedirectToAction(nameof(Index));

            return View(new ValidateOtpViewModel { Code = string.Empty });
        }

        // 4 Verificar OTP
        [HttpPost]
        public async Task<IActionResult> ValidateOtp(ValidateOtpViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var citizenId = HttpContext.Session.Get<int>("CitizenId");
            var electionId = HttpContext.Session.Get<int>("ElectionId");

            var result = await _electorService.ValidateOtpAsync(citizenId, electionId, vm.Code);

            switch (result)
            {
                case OtpValidationResult.Expired:
                    ModelState.AddModelError("Code", "El código de verificación ha expirado. Solicite un nuevo código para continuar.");
                    return View(vm);
                case OtpValidationResult.AlreadyUsed:
                    ModelState.AddModelError("Code", "Este código de verificación ya fue utilizado.");
                    return View(vm);
                case OtpValidationResult.Invalid:
                case OtpValidationResult.NotFound:
                    ModelState.AddModelError("Code", "El código de verificación ingresado no es válido.");
                    return View(vm);
            }

            HttpContext.Session.Set("OtpValidated", true);
            return RedirectToAction(nameof(Voting));
        }
     
        // 5 Pantalla de votación
        public async Task<IActionResult> Voting()
        {
            if (!HttpContext.Session.Get<bool>("OtpValidated"))
                return RedirectToAction(nameof(Index));

            var positions = await _electivePositionService.GetPositionsWithCandidatesForVotingAsync();

            var vm = new VotingViewModel
            {
                Positions = positions.Select(p => new ElectivePositionVoteViewModel
                {
                    ElectivePositionId = p.Id,
                    PositionName = p.Name,
                    Candidates = p.Candidates.Select(c => new CandidateOptionViewModel
                    {
                        Id = c.Id,
                        FullName = c.FullName,
                        PartyName = c.PartyName
                    }).ToList()
                }).ToList()
            };

            // Restaurar selecciones previas si el elector volvió a cambiar algo
            var selections = HttpContext.Session.Get<Dictionary<int, SaveVoteDto>>("VoteSelections");
            if (selections != null)
            {
                foreach (var pos in vm.Positions)
                {
                    if (selections.TryGetValue(pos.ElectivePositionId, out var saved))
                        pos.SelectedCandidateId = saved.CandidateId;
                }
            }

            return View(vm);
        }

        // 6 Guardar selección
        [HttpPost]
        public IActionResult SavePositionVote(int electivePositionId, int? candidateId, string positionName, string candidateName, string partyName)
        {
            if (!HttpContext.Session.Get<bool>("OtpValidated"))
                return RedirectToAction(nameof(Index));

            var selections = HttpContext.Session.Get<Dictionary<int, SaveVoteDto>>("VoteSelections")
                             ?? new Dictionary<int, SaveVoteDto>();

            selections[electivePositionId] = new SaveVoteDto
            {
                ElectivePositionId = electivePositionId,
                CandidateId = candidateId,
                PositionName = positionName,
                CandidateName = candidateName,
                PartyName = partyName
            };

            HttpContext.Session.Set("VoteSelections", selections);
            return RedirectToAction(nameof(Voting));
        }

        // 7 Finalizar votación
        [HttpPost]
        public async Task<IActionResult> FinishVoting()
        {
            if (!HttpContext.Session.Get<bool>("OtpValidated"))
                return RedirectToAction(nameof(Index));

            var citizenId = HttpContext.Session.Get<int>("CitizenId");
            var electionId = HttpContext.Session.Get<int>("ElectionId");
            var citizenName = HttpContext.Session.Get<string>("CitizenName");
            var email = HttpContext.Session.Get<string>("CitizenEmail");
            var selections = HttpContext.Session.Get<Dictionary<int, SaveVoteDto>>("VoteSelections");

            var totalPositions = (await _electivePositionService.GetPositionsWithCandidatesForVotingAsync()).Count;

            if (selections == null || selections.Count != totalPositions)
            {
                TempData["Error"] = "Debe completar su selección para todos los puestos electivos.";
                return RedirectToAction(nameof(Voting));
            }

            var votes = selections.Values.Select(s => new SaveVoteDto
            {
                ElectionId = electionId,
                ElectivePositionId = s.ElectivePositionId,
                CandidateId = s.CandidateId,
                PositionName = s.PositionName,
                CandidateName = s.CandidateName,
                PartyName = s.PartyName
            }).ToList();

            // Obtener nombre y fecha de elección
            var elections = await _electionService.GetAllAsync();
            var activeElection = elections.FirstOrDefault(e => e.Status == ElectionStatus.Active);
            string electionName = activeElection?.Name ?? "Elección activa";
            string electionDate = activeElection?.ActivationDate?.ToString("dd/MM/yyyy") ?? DateTime.Now.ToString("dd/MM/yyyy");

            bool success = await _electorService.ConfirmVoteAsync(citizenId, electionId, votes, citizenName!, electionName, electionDate, email!);

            if (!success)
            {
                TempData["Error"] = "Ocurrió un error al registrar el voto. Intente nuevamente.";
                return RedirectToAction(nameof(Voting));
            }

            HttpContext.Session.Remove("DocumentoIngresado");
            HttpContext.Session.Remove("ElectionId");
            HttpContext.Session.Remove("CitizenId");
            HttpContext.Session.Remove("CitizenName");
            HttpContext.Session.Remove("CitizenEmail");
            HttpContext.Session.Remove("OcrValidated");
            HttpContext.Session.Remove("OtpValidated");
            HttpContext.Session.Remove("VoteSelections");

            return RedirectToAction(nameof(Confirmation));
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}