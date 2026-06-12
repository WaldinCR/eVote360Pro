using eVote360Pro.Core.Application.Dtos.Email;
using eVote360Pro.Core.Application.Dtos.Vote;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Core.Domain.Common.Enums;
using AutoMapper;

namespace eVote360Pro.Core.Application.Services
{
    public class ElectorService : IElectorService
    {
        private readonly IVerificationCodeRepository _verificationCodeRepo;
        private readonly ICitizenVoteRepository _citizenVoteRepo;
        private readonly IVoteRepository _voteRepo;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public ElectorService(
            IVerificationCodeRepository verificationCodeRepo,
            ICitizenVoteRepository citizenVoteRepo,
            IVoteRepository voteRepo,
            IEmailService emailService,
            IMapper mapper)
        {
            _verificationCodeRepo = verificationCodeRepo;
            _citizenVoteRepo = citizenVoteRepo;
            _voteRepo = voteRepo;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<bool> GenerateAndSendOtpAsync(int citizenId, int electionId, string citizenName, string email)
        {
            var code = new Random().Next(100000, 999999).ToString();

            var verificationCode = new VerificationCode
            {
                CitizenId = citizenId,
                ElectionId = electionId,
                Code = code,
                GeneratedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            await _verificationCodeRepo.AddAsync(verificationCode);

            bool sent = await _emailService.SendAsync(new EmailRequestDto
            {
                To = email,
                Subject = "Código de verificación para votar",
                HtmlBody = $@"
                    <p>Hola {citizenName},</p>
                    <p>Su código de verificación para continuar con el proceso de votación es:</p>
                    <h1 style='letter-spacing:8px;color:#3C3489;'>{code}</h1>
                    <p>Este código tendrá una vigencia de <strong>5 minutos</strong>.</p>
                    <p>Si usted no inició este proceso, ignore este mensaje.</p>"
            });

            return sent;
        }

        public async Task<OtpValidationResult> ValidateOtpAsync(int citizenId, int electionId, string code)
        {
            var verificationCode = await _verificationCodeRepo.GetActiveCodeByCodeAsync(citizenId, electionId, code);

            if (verificationCode == null)
                return OtpValidationResult.NotFound;

            if (verificationCode.IsUsed)
                return OtpValidationResult.AlreadyUsed;

            if (verificationCode.ExpiresAt < DateTime.UtcNow)
                return OtpValidationResult.Expired;

            if (verificationCode.Code != code)
                return OtpValidationResult.Invalid;

            await _verificationCodeRepo.MarkAsUsedAsync(verificationCode.Id);
            return OtpValidationResult.Success;
        }

        public async Task<bool> ConfirmVoteAsync(int citizenId, int electionId, List<SaveVoteDto> votes, string citizenName, string electionName, string electionDate, string email)
        {
            try
            {
                // Guardar votos — sin ligar al ciudadano (confidencialidad)
                foreach (var voteDto in votes)
                {
                    var vote = new Vote
                    {
                        ElectionId = electionId,
                        ElectivePositionId = voteDto.ElectivePositionId,
                        CandidateId = voteDto.CandidateId
                    };
                    await _voteRepo.AddAsync(vote);
                }

                // Registrar participación del ciudadano
                var citizenVote = new CitizenVote
                {
                    CitizenId = citizenId,
                    ElectionId = electionId,
                    VotedAt = DateTime.UtcNow
                };
                await _citizenVoteRepo.AddAsync(citizenVote);

                var selectionRows = string.Join("", votes.Select(v =>
                    $"<tr><td>{v.PositionName}</td><td>{(v.CandidateName ?? "Ninguno")}</td><td>{(v.PartyName ?? "-")}</td></tr>"));

                await _emailService.SendAsync(new EmailRequestDto
                {
                    To = email,
                    Subject = "Resumen de su participación electoral",
                    HtmlBody = $@"
                        <p>Hola {citizenName},</p>
                        <p>Su proceso de votación ha sido completado correctamente.</p>
                        <p><strong>Elección:</strong> {electionName}</p>
                        <p><strong>Fecha de la elección:</strong> {electionDate}</p>
                        <h3>Resumen de selección:</h3>
                        <table border='1' cellpadding='6' cellspacing='0' style='border-collapse:collapse;'>
                            <thead>
                                <tr><th>Puesto</th><th>Selección</th><th>Partido</th></tr>
                            </thead>
                            <tbody>{selectionRows}</tbody>
                        </table>
                        <br/>
                        <p>Gracias por ejercer su derecho al voto.</p>"
                });

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}