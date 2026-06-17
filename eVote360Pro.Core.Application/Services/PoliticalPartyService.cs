using eVote360Pro.Core.Application.Dtos.PoliticalParty;
using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.PoliticalParty;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces.Repositories;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace eVote360Pro.Core.Application.Services
{
    public class PoliticalPartyService : GenericService<PoliticalParty, SavePoliticalPartyDto>, IPoliticalPartyService
    {
        private readonly IPoliticalPartyRepository _partyRepository;
        private readonly IGenericRepository<Election> _electionRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IPoliticalLeaderAssignmentRepository _assignmentRepository;
        private readonly IMapper _mapper;

        public PoliticalPartyService(
            IPoliticalPartyRepository partyRepository,
            IGenericRepository<Election> electionRepository,
            ICandidateRepository candidateRepository,
            IPoliticalLeaderAssignmentRepository assignmentRepository,
            IMapper mapper) : base(partyRepository, mapper)
        {
            _partyRepository = partyRepository;
            _electionRepository = electionRepository;
            _candidateRepository = candidateRepository;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }

        public new async Task<List<PoliticalPartyDto>> GetAllAsync()
        {
            var parties = await _partyRepository.GetAllAsync();
            return _mapper.Map<List<PoliticalPartyDto>>(parties);
        }

        public async Task<SavePoliticalPartyDto?> GetByIdSaveDtoAsync(int id)
        {
            var party = await _partyRepository.GetByIdAsync(id);
            if (party == null) return null;
            return _mapper.Map<SavePoliticalPartyDto>(party);
        }

        public override async Task<SavePoliticalPartyDto?> AddAsync(SavePoliticalPartyDto dto)
        {
            // Bloqueo por elección activa
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active))
                throw new Exception("No se puede crear un partido político mientras haya una elección activa.");

            // Validar siglas únicas
            var all = await _partyRepository.GetAllAsync();

            if (all.Any(p => p.Acronym.ToLower() == dto.Acronym.ToLower()))
                throw new Exception($"Ya existe un partido con las siglas '{dto.Acronym}'.");

            if (all.Any(p => p.Name.ToLower() == dto.Name.ToLower()))
                throw new Exception($"Ya existe un partido con el nombre '{dto.Name}'.");

            var entity = _mapper.Map<PoliticalParty>(dto);
            entity.IsActive = dto.IsActive;
            entity.Description = dto.Description;

            var savedEntity = await _partyRepository.AddAsync(entity);
            return _mapper.Map<SavePoliticalPartyDto>(savedEntity);
        }

        public async Task UpdateAsync(SavePoliticalPartyDto dto)
        {
            // Bloqueo por elección activa
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active))
                throw new Exception("No se puede editar un partido político mientras haya una elección activa.");

            var entity = await _partyRepository.GetByIdAsync(dto.Id);
            if (entity == null) throw new Exception("Partido no encontrado.");

            // Validación de edición por participación en elección activa o finalizada
            var candidates = await _candidateRepository.GetAllAsync();
            var partyCandidates = candidates.Where(c => c.PoliticalPartyId == dto.Id).ToList();
            bool hasParticipated = partyCandidates.Any(c => c.ParticipatedInElection);

            if (hasParticipated)
            {
                if (entity.Name != dto.Name || entity.Acronym != dto.Acronym || entity.LogoUrl != dto.LogoUrl)
                {
                    throw new Exception("No se puede editar el nombre, siglas o logo de un partido que ha participado en una elección activa o finalizada.");
                }
            }

            var all = await _partyRepository.GetAllAsync();

            if (all.Any(p => p.Acronym.ToLower() == dto.Acronym.ToLower() && p.Id != dto.Id))
                throw new Exception($"Ya existe un partido con las siglas '{dto.Acronym}'.");

            if (all.Any(p => p.Name.ToLower() == dto.Name.ToLower() && p.Id != dto.Id))
                throw new Exception($"Ya existe un partido con el nombre '{dto.Name}'.");

            _mapper.Map(dto, entity);
            entity.IsActive = dto.IsActive;
            entity.Description = dto.Description;

            await _partyRepository.UpdateAsync(entity);
        }

        public async Task ChangeStatusAsync(int id)
        {
            // Bloqueo por elección activa
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active))
                throw new Exception("No se puede cambiar el estado de un partido político mientras haya una elección activa.");

            var entity = await _partyRepository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Partido no encontrado.");

            if (entity.IsActive)
            {
                var candidates = await _candidateRepository.GetAllAsync();
                if (candidates.Any(c => c.PoliticalPartyId == id && c.IsActive))
                    throw new Exception("No se puede desactivar este partido político porque tiene candidatos activos registrados.");

                var assignments = await _assignmentRepository.GetAllAsync();
                if (assignments.Any(a => a.PoliticalPartyId == id))
                    throw new Exception("No se puede desactivar este partido político porque tiene un dirigente político activo asignado.");
            }

            entity.IsActive = !entity.IsActive;
            await _partyRepository.UpdateAsync(entity);
        }
    }
}