using eVote360Pro.Core.Application.Dtos.Alliance;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Alliance;
using eVote360Pro.Core.Domain.Common.Enums;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Core.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace eVote360Pro.Core.Application.Services
{
    public class AllianceRequestService : GenericService<AllianceRequest, CreateAllianceRequestViewModel>, IAllianceRequestService
    {
        private readonly IAllianceRequestRepository _requestRepository;
        private readonly IAllianceRepository _allianceRepository;
        private readonly IPoliticalPartyRepository _partyRepository;
        private readonly IGenericRepository<Election> _electionRepository;
        private readonly IMapper _mapper;

        public AllianceRequestService(
            IAllianceRequestRepository requestRepository,
            IAllianceRepository allianceRepository,
            IPoliticalPartyRepository partyRepository,
            IGenericRepository<Election> electionRepository,
            IMapper mapper) : base(requestRepository, mapper)
        {
            _requestRepository = requestRepository;
            _allianceRepository = allianceRepository;
            _partyRepository = partyRepository;
            _electionRepository = electionRepository;
            _mapper = mapper;
        }

        public async Task<List<AllianceRequestViewModel>> GetAllViewModel()
        {
            var requests = await _requestRepository.GetAllAsync();
            return await MapToViewModelList(requests);
        }

        public async Task<List<AllianceRequestViewModel>> GetReceivedPendingAsync(int receiverPartyId)
        {
            var requests = await _requestRepository.GetAllAsync();
            return await MapToViewModelList(
                requests.Where(r => r.ReceiverPartyId == receiverPartyId
                    && r.Status == AllianceRequestStatus.Pending).ToList());
        }

        public async Task<List<AllianceRequestViewModel>> GetSentByPartyAsync(int applicantPartyId)
        {
            var requests = await _requestRepository.GetAllAsync();
            return await MapToViewModelList(
                requests.Where(r => r.ApplicantPartyId == applicantPartyId).ToList());
        }

        public override async Task<CreateAllianceRequestViewModel?> AddAsync(CreateAllianceRequestViewModel vm)
        {
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active))
                throw new Exception("No se pueden enviar solicitudes de alianza mientras haya una elección activa.");

            if (vm.ApplicantPartyId == vm.ReceiverPartyId)
                throw new Exception("No puedes solicitar una alianza con tu propio partido.");

            if (await HasPendingRequestAsync(vm.ApplicantPartyId, vm.ReceiverPartyId))
                throw new Exception("Ya existe una solicitud pendiente entre estos partidos.");

            var alliances = await _allianceRepository.GetAllAsync();
            bool allianceExists = alliances.Any(a =>
                (a.Party1Id == vm.ApplicantPartyId && a.Party2Id == vm.ReceiverPartyId) ||
                (a.Party1Id == vm.ReceiverPartyId && a.Party2Id == vm.ApplicantPartyId));

            if (allianceExists)
                throw new Exception("Ya existe una alianza vigente entre estos partidos.");

            var entity = new AllianceRequest
            {
                ApplicantPartyId = vm.ApplicantPartyId,
                ReceiverPartyId = vm.ReceiverPartyId,
                Status = AllianceRequestStatus.Pending,
                RequestDate = DateTime.Now
            };

            var savedEntity = await _requestRepository.AddAsync(entity);
            return new CreateAllianceRequestViewModel
            {
                ApplicantPartyId = savedEntity.ApplicantPartyId,
                ReceiverPartyId = savedEntity.ReceiverPartyId
            };
        }

        public async Task AcceptRequestAsync(int id, int receiverPartyId)
        {
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active))
                throw new Exception("No se pueden aceptar solicitudes de alianza mientras haya una elección activa.");

            var request = await _requestRepository.GetByIdAsync(id);
            if (request == null) throw new Exception("Solicitud no encontrada.");
            if (request.ReceiverPartyId != receiverPartyId)
                throw new Exception("Solo el partido receptor puede aceptar esta solicitud.");
            if (request.Status != AllianceRequestStatus.Pending)
                throw new Exception("Solo se pueden aceptar solicitudes pendientes.");

            request.Status = AllianceRequestStatus.Accepted;
            request.ResponseDate = DateTime.Now;
            await _requestRepository.UpdateAsync(request);

            var alliance = new Alliance
            {
                Party1Id = request.ApplicantPartyId,
                Party2Id = request.ReceiverPartyId,
                CreationDate = DateTime.Now
            };
            await _allianceRepository.AddAsync(alliance);
        }

        public async Task RejectRequestAsync(int id, int receiverPartyId)
        {
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active))
                throw new Exception("No se pueden rechazar solicitudes de alianza mientras haya una elección activa.");

            var request = await _requestRepository.GetByIdAsync(id);
            if (request == null) throw new Exception("Solicitud no encontrada.");
            if (request.ReceiverPartyId != receiverPartyId)
                throw new Exception("Solo el partido receptor puede rechazar esta solicitud.");
            if (request.Status != AllianceRequestStatus.Pending)
                throw new Exception("Solo se pueden rechazar solicitudes pendientes.");

            request.Status = AllianceRequestStatus.Rejected;
            request.ResponseDate = DateTime.Now;
            await _requestRepository.UpdateAsync(request);
        }

        public async Task DeleteAsync(int id, int partyId)
        {
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active))
                throw new Exception("No se pueden eliminar solicitudes de alianza mientras haya una elección activa.");

            var entity = await _requestRepository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Solicitud no encontrada.");
            
            if (entity.ApplicantPartyId != partyId)
                throw new Exception("Solo puedes eliminar solicitudes enviadas por tu propio partido.");

            if (entity.Status != AllianceRequestStatus.Pending)
                throw new Exception("Solo se pueden eliminar solicitudes pendientes.");

            await _requestRepository.DeleteAsync(entity);
        }

        public async Task<bool> HasPendingRequestAsync(int applicantId, int receiverId)
        {
            var requests = await _requestRepository.GetAllAsync();
            return requests.Any(r =>
                r.Status == AllianceRequestStatus.Pending &&
                ((r.ApplicantPartyId == applicantId && r.ReceiverPartyId == receiverId) ||
                 (r.ApplicantPartyId == receiverId && r.ReceiverPartyId == applicantId)));
        }

        private async Task<List<AllianceRequestViewModel>> MapToViewModelList(List<AllianceRequest> requests)
        {
            var parties = await _partyRepository.GetAllAsync();

            var dtos = requests.Select(r => new AllianceRequestDto
            {
                Id = r.Id,
                ApplicantPartyId = r.ApplicantPartyId,
                ApplicantPartyName = parties.FirstOrDefault(p => p.Id == r.ApplicantPartyId)?.Name ?? "Desconocido",
                ReceiverPartyId = r.ReceiverPartyId,
                ReceiverPartyName = parties.FirstOrDefault(p => p.Id == r.ReceiverPartyId)?.Name ?? "Desconocido",
                Status = (int)r.Status,
                RequestDate = r.RequestDate.ToString("dd/MM/yyyy")
            }).ToList();

            return dtos.Select(d => new AllianceRequestViewModel
            {
                Id = d.Id,
                ApplicantPartyId = d.ApplicantPartyId,
                ApplicantPartyName = d.ApplicantPartyName,
                ReceiverPartyId = d.ReceiverPartyId,
                ReceiverPartyName = d.ReceiverPartyName,
                Status = (AllianceRequestStatus)d.Status,
                RequestDate = DateTime.Parse(d.RequestDate),
                ResponseDate = null
            }).ToList();
        }
    }
}