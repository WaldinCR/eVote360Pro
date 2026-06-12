///using eVote360Pro.App.Filters;
using eVote360Pro.Core.Application.Dtos.Vote;
using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Elector;
using eVote360Pro.Core.Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.App.Areas.Elector.Controllers
{
    [Area("Elector")]
    public class ElectorController : Controller
    {
        private readonly IElectorService _electorService;
        private readonly IOcrService _ocrService;

        public ElectorController(IElectorService electorService, IOcrService ocrService)
        {
            _electorService = electorService;
            _ocrService = ocrService;
        }

        // Ingresar documento 
        public IActionResult Index()
        {
            // si hay elección activa, mostrar formulario. Si no, mostrar mensaje.
            // bool hasActiveElection = await _electionService.HasActiveElectionAsync();
            // if (!hasActiveElection) { ViewBag.Message = "No hay ningún proceso electoral en estos momentos."; }
            return View(new AddDocumentViewModel { Document = string.Empty });
        }

        [HttpPost]
        public async Task<IActionResult> Index(AddDocumentViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // reemplazar con llamadas reales al servicio !!!!
            // var activeElection = await _electionService.GetActiveElectionAsync();
            // if (activeElection == null) { ModelState.AddModelError("", "No hay ningún proceso electoral en estos momentos."); return View(vm); }

            // var citizen = await _citizenService.GetByDocumentAsync(vm.Document.Trim());
            // if (citizen == null) { ModelState.AddModelError("Document", "No existe un ciudadano registrado con este número de documento."); return View(vm); }
            // if (!citizen.IsActive) { ModelState.AddModelError("Document", "Este ciudadano se encuentra inactivo y no puede participar en el proceso de votación."); return View(vm); }

            // var alreadyVoted = await _citizenVoteRepo.HasAlreadyVotedAsync(citizen.Id, activeElection.Id);
            // if (alreadyVoted) { ModelState.AddModelError("Document", "Ya ha ejercido su derecho al voto."); return View(vm); }

            // if (string.IsNullOrWhiteSpace(citizen.Email)) { ModelState.AddModelError("", "Este ciudadano no tiene un correo electrónico registrado. No es posible continuar con la verificación de identidad."); return View(vm); }

            // Valores de prueba mientras tanto (arreglar despues):
            int citizenId = 1;
            int electionId = 1;
            string citizenName = "Ciudadano";
            string citizenEmail = "test@test.com";

            HttpContext.Session.Set("DocumentoIngresado", vm.Document.Trim());
            HttpContext.Session.Set("ElectionId", electionId);
            HttpContext.Session.Set("CitizenId", citizenId);
            HttpContext.Session.Set("CitizenName", citizenName);
            HttpContext.Session.Set("CitizenEmail", citizenEmail);

            return RedirectToAction(nameof(LoadCedula));
        }

        // Cargar cédula 
        public IActionResult LoadCedula()
        {
            var document = HttpContext.Session.Get<string>("DocumentoIngresado");
            if (string.IsNullOrEmpty(document))
                return RedirectToAction(nameof(Index));

            return View(new LoadCedulaViewModel { Document = document, CedulaImage = null });
        }

        [HttpPost]
        public async Task<IActionResult> LoadCedula(LoadCedulaViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // Validar formato
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

            // Procesar OCR
            var extractedText = await _ocrService.ExtractDocumentNumberAsync(vm.CedulaImage!);

            if (string.IsNullOrWhiteSpace(extractedText))
            {
                ModelState.AddModelError("CedulaImage", "No fue posible leer correctamente el número de documento en la imagen cargada. Por favor, suba una imagen más clara.");
                return View(vm);
            }

            // Comparar con documento ingresado
            var normalizedExtracted = extractedText.Replace("-", "").Replace(" ", "").Trim();
            var normalizedExpected = expectedDocument!.Replace("-", "").Replace(" ", "").Trim();

            if (!normalizedExtracted.Contains(normalizedExpected))
            {
                ModelState.AddModelError("CedulaImage", "Los datos extraídos de la foto no coinciden con los datos previamente ingresados por el elector.");
                return View(vm);
            }

            // OCR exitoso — generar y enviar OTP
            bool sent = await _electorService.GenerateAndSendOtpAsync(citizenId, electionId, citizenName!, email!);
            if (!sent)
            {
                ModelState.AddModelError("", "No fue posible enviar el código de verificación. Intente nuevamente más tarde.");
                return View(vm);
            }

            HttpContext.Session.Set("OcrValidated", true);
            return RedirectToAction(nameof(ValidateOtp));
        }

        // Validar OTP 
        public IActionResult ValidateOtp()
        {
            if (!HttpContext.Session.Get<bool>("OcrValidated"))
                return RedirectToAction(nameof(Index));

            return View(new ValidateOtpViewModel { Code = string.Empty });
        }

        [HttpPost]
        //public async Task<IActionResult> ValidateOtp(ValidateOtpViewModel vm)
        //{
        //    if (!ModelState.IsValid) return View(vm);

        //    var citizenId = HttpContext.Session.Get<int>("CitizenId");
        //    var electionId = HttpContext.Session.Get<int>("ElectionId");

        //    var result = await _electorService.ValidateOtpAsync(citizenId, electionId, vm.Code);

        //    switch (result)
        //    {
        //        case OtpValidationResult.Expired:
        //            ModelState.AddModelError("Code", "El código de verificación ha expirado. Solicite un nuevo código para continuar.");
        //            return View(vm);
        //        case OtpValidationResult.AlreadyUsed:
        //            ModelState.AddModelError("Code", "Este código de verificación ya fue utilizado.");
        //            return View(vm);
        //        case OtpValidationResult.Invalid:
        //        case OtpValidationResult.NotFound:
        //            ModelState.AddModelError("Code", "El código de verificación ingresado no es válido.");
        //            return View(vm);
        //    }

        //    HttpContext.Session.Set("OtpValidated", true);
        //    return RedirectToAction(nameof(Voting));
        //}

        // Pantalla de puestos electivos 
        //public IActionResult Voting()
        ////{
        //   if (!HttpContext.Session.Get<bool>("OtpValidated"))
        //       return RedirectToAction(nameof(Index));

        //    // cargar puestos activos de la elección con sus candidatos
        //    // var positions = await _electionService.GetActivePositionsAsync(electionId);
        //    return View(new VotingViewModel());
        //}

        // Guardar selección de un puesto
        [HttpPost]
        //public IActionResult SavePositionVote(int electivePositionId, int? candidateId, string positionName, string candidateName, string partyName)
        //{
        //    if (!HttpContext.Session.Get<bool>("OtpValidated"))
        //        return RedirectToAction(nameof(Index));

        //    // Guardar selección en sesión
        //    var selections = HttpContext.Session.Get<Dictionary<int, SaveVoteDto>>("VoteSelections")
        //                     ?? new Dictionary<int, SaveVoteDto>();

        //    selections[electivePositionId] = new SaveVoteDto
        //    {
        //        ElectivePositionId = electivePositionId,
        //        CandidateId = candidateId,
        //        PositionName = positionName,
        //        CandidateName = candidateName,
        //        PartyName = partyName
        //    };

        //    HttpContext.Session.Set("VoteSelections", selections);
        //    return RedirectToAction(nameof(Voting));
        //}

        //  Finalizar votación 
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

            // validar que todos los puestos tienen selección
            // var totalPositions = await _electionService.GetActivePositionsCountAsync(electionId);
            // var missingPositions = ...
            // if (missingPositions.Any()) { TempData["Error"] = $"Debe completar su selección para los siguientes puestos electivos: {string.Join(", ", missingPositions)}"; return RedirectToAction(nameof(Voting)); }

            //if (selections == null || !selections.Any())
            //{
            //    TempData["Error"] = "Debe completar su selección para todos los puestos electivos.";
            //    return RedirectToAction(nameof(Voting));
            //}

            var votes = selections.Values.Select(s => new SaveVoteDto
            {
                ElectionId = electionId,
                ElectivePositionId = s.ElectivePositionId,
                CandidateId = s.CandidateId,
                PositionName = s.PositionName,
                CandidateName = s.CandidateName,
                PartyName = s.PartyName
            }).ToList();

            //  obtener nombre y fecha de elección del servicio
            string electionName = "Elección activa";
            string electionDate = DateTime.Now.ToString("dd/MM/yyyy");

            bool success = await _electorService.ConfirmVoteAsync(citizenId, electionId, votes, citizenName!, electionName, electionDate, email!);

            //if (!success)
            //{
            //    TempData["Error"] = "Ocurrió un error al registrar el voto. Intente nuevamente.";
            //    return RedirectToAction(nameof(Voting));
            //}

            // Limpiar toda la sesión del elector
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
