using eVote360Pro.Core.Application.Dtos.Candidate;
using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Candidate;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Core.Domain.Interfaces.Repositories;
using eVote360Pro.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace eVote360Pro.Core.Application.Services
{
    public class CandidateService : GenericService<Candidate, SaveCandidateViewModel>, ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IPoliticalPartyRepository _partyRepository;
        private readonly IGenericRepository<Election> _electionRepository;
        private readonly IGenericRepository<CandidatePosition> _candidatePositionRepository;
        private readonly IMapper _mapper;

        public CandidateService(
            ICandidateRepository candidateRepository, 
            IPoliticalPartyRepository partyRepository,
            IGenericRepository<Election> electionRepository,
            IGenericRepository<CandidatePosition> candidatePositionRepository,
            IMapper mapper) : base(candidateRepository, mapper)
        {
            _candidateRepository = candidateRepository;
            _partyRepository = partyRepository;
            _electionRepository = electionRepository;
            _candidatePositionRepository = candidatePositionRepository;
            _mapper = mapper;
        }

        public async Task<List<CandidateViewModel>> GetAllViewModel()
        {
            var candidates = await _candidateRepository.GetAllAsync();
            var parties = await _partyRepository.GetAllAsync();
            return MapToViewModelList(candidates, parties);
        }

        public async Task<List<CandidateViewModel>> GetAllByPartyIdViewModel(int partyId)
        {
            var candidates = await _candidateRepository.GetAllAsync();
            var parties = await _partyRepository.GetAllAsync();
            return MapToViewModelList(
                candidates.Where(c => c.PoliticalPartyId == partyId).ToList(), parties);
        }

        public async Task<SaveCandidateViewModel?> GetByIdSaveViewModel(int id)
        {
            var c = await _candidateRepository.GetByIdAsync(id);
            if (c == null) return null;

            // Entity → SaveDto → SaveViewModel
            var dto = new SaveCandidateDto
            {
                Id = c.Id,
                Name = c.Name,
                LastName = c.LastName,
                PhotoUrl = c.PhotoUrl,
                IsActive = c.IsActive,
                PoliticalPartyId = c.PoliticalPartyId
            };

            return new SaveCandidateViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                LastName = dto.LastName,
                PhotoUrl = dto.PhotoUrl,
                IsActive = dto.IsActive,
                PoliticalPartyId = dto.PoliticalPartyId
            };
        }

        public override async Task<SaveCandidateViewModel?> AddAsync(SaveCandidateViewModel vm)
        {
            // Bloqueo por elección activa
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active))
                throw new Exception("No se puede crear un candidato mientras haya una elección activa.");

            var entity = new Candidate
            {
                Name = vm.Name,
                LastName = vm.LastName,
                PhotoUrl = vm.PhotoUrl ?? string.Empty,
                IsActive = true,
                ParticipatedInElection = false,
                PoliticalPartyId = vm.PoliticalPartyId
            };

            var savedEntity = await _candidateRepository.AddAsync(entity);

            return new SaveCandidateViewModel
            {
                Id = savedEntity.Id,
                Name = savedEntity.Name,
                LastName = savedEntity.LastName,
                PhotoUrl = savedEntity.PhotoUrl,
                IsActive = savedEntity.IsActive,
                PoliticalPartyId = savedEntity.PoliticalPartyId
            };
        }

        public async Task UpdateAsync(SaveCandidateViewModel vm)
        {
            // Bloqueo por elección activa
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active))
                throw new Exception("No se puede editar un candidato mientras haya una elección activa.");

            var entity = await _candidateRepository.GetByIdAsync(vm.Id);
            if (entity == null) throw new Exception("Candidato no encontrado.");

            if (entity.ParticipatedInElection)
                throw new Exception("No se puede editar un candidato que participó en una elección activa o finalizada.");

            entity.Name = vm.Name;
            entity.LastName = vm.LastName;

            if (!string.IsNullOrWhiteSpace(vm.PhotoUrl))
            {
                entity.PhotoUrl = vm.PhotoUrl;
            }

            await _candidateRepository.UpdateAsync(entity);
        }

        public async Task ChangeStatusAsync(int id, bool status)
        {
            // Bloqueo por elección activa
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active))
                throw new Exception("No se puede cambiar el estado de un candidato mientras haya una elección activa.");

            var entity = await _candidateRepository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Candidato no encontrado.");

            if (!status)
            {
                var assignments = await _candidatePositionRepository.GetAllAsync();
                if (assignments.Any(cp => cp.CandidateId == id))
                    throw new Exception("No se puede desactivar un candidato que está asignado a un puesto electivo.");
            }

            entity.IsActive = status;
            await _candidateRepository.UpdateAsync(entity);
        }

        private List<CandidateViewModel> MapToViewModelList(List<Candidate> candidates, List<PoliticalParty> parties)
        {
            var dtos = candidates.Select(c => new CandidateDto
            {
                Id = c.Id,
                Name = c.Name,
                LastName = c.LastName,
                PhotoUrl = c.PhotoUrl,
                IsActive = c.IsActive,
                PoliticalPartyId = c.PoliticalPartyId,
                PoliticalPartyName = parties.FirstOrDefault(p => p.Id == c.PoliticalPartyId)?.Name ?? "Desconocido"
            }).ToList();

            return dtos.Select(d => new CandidateViewModel
            {
                Id = d.Id,
                Name = d.Name,
                LastName = d.LastName,
                PhotoUrl = d.PhotoUrl,
                IsActive = d.IsActive,
                PoliticalPartyId = d.PoliticalPartyId,
                PoliticalPartyName = d.PoliticalPartyName
            }).ToList();
        }
    }
}