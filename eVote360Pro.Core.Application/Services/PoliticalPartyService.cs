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
    public class PoliticalPartyService : GenericService<PoliticalParty, SavePoliticalPartyViewModel>, IPoliticalPartyService
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

        public async Task<List<PoliticalPartyViewModel>> GetAllViewModel()
        {
            var parties = await _partyRepository.GetAllAsync();

            var dtos = parties.Select(p => new PoliticalPartyDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Acronym = p.Acronym,
                LogoUrl = p.LogoUrl,
                IsActive = p.IsActive
            }).ToList();

            return dtos.Select(d => new PoliticalPartyViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Acronym = d.Acronym,
                LogoUrl = d.LogoUrl,
                IsActive = d.IsActive
            }).ToList();
        }

        public async Task<SavePoliticalPartyViewModel?> GetByIdSaveViewModel(int id)
        {
            var party = await _partyRepository.GetByIdAsync(id);
            if (party == null) return null;

            var dto = new SavePoliticalPartyDto
            {
                Id = party.Id,
                Name = party.Name,
                Description = party.Description,
                Acronym = party.Acronym,
                LogoUrl = party.LogoUrl,
                IsActive = party.IsActive
            };

            return new SavePoliticalPartyViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Acronym = dto.Acronym,
                LogoUrl = dto.LogoUrl,
                IsActive = dto.IsActive
            };
        }

        public override async Task<SavePoliticalPartyViewModel?> AddAsync(SavePoliticalPartyViewModel vm)
        {
            // Bloqueo por elección activa
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active))
                throw new Exception("No se puede crear un partido político mientras haya una elección activa.");

            // Validar siglas únicas
            var all = await _partyRepository.GetAllAsync();

            if (all.Any(p => p.Acronym.ToLower() == vm.Acronym.ToLower()))
                throw new Exception($"Ya existe un partido con las siglas '{vm.Acronym}'.");

            if (all.Any(p => p.Name.ToLower() == vm.Name.ToLower()))
                throw new Exception($"Ya existe un partido con el nombre '{vm.Name}'.");

            var entity = new PoliticalParty
            {
                Name = vm.Name,
                Description = vm.Description,
                Acronym = vm.Acronym,
                LogoUrl = vm.LogoUrl ?? string.Empty,
                IsActive = true
            };

            var savedEntity = await _partyRepository.AddAsync(entity);

            return new SavePoliticalPartyViewModel
            {
                Id = savedEntity.Id,
                Name = savedEntity.Name,
                Description = savedEntity.Description,
                Acronym = savedEntity.Acronym,
                LogoUrl = savedEntity.LogoUrl,
                IsActive = savedEntity.IsActive
            };
        }

        public async Task UpdateAsync(SavePoliticalPartyViewModel vm)
        {
            // Bloqueo por elección activa
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active))
                throw new Exception("No se puede editar un partido político mientras haya una elección activa.");

            var entity = await _partyRepository.GetByIdAsync(vm.Id);
            if (entity == null) throw new Exception("Partido no encontrado.");

            // Validación de edición por participación en elección activa o finalizada
            var candidates = await _candidateRepository.GetAllAsync();
            var partyCandidates = candidates.Where(c => c.PoliticalPartyId == vm.Id).ToList();
            bool hasParticipated = partyCandidates.Any(c => c.ParticipatedInElection);

            if (hasParticipated)
            {
                if (entity.Name != vm.Name || entity.Acronym != vm.Acronym || vm.LogoFile != null)
                {
                    throw new Exception("No se puede editar el nombre, siglas o logo de un partido que ha participado en una elección activa o finalizada.");
                }
            }

            var all = await _partyRepository.GetAllAsync();

            if (all.Any(p => p.Acronym.ToLower() == vm.Acronym.ToLower() && p.Id != vm.Id))
                throw new Exception($"Ya existe un partido con las siglas '{vm.Acronym}'.");

            if (all.Any(p => p.Name.ToLower() == vm.Name.ToLower() && p.Id != vm.Id))
                throw new Exception($"Ya existe un partido con el nombre '{vm.Name}'.");

            entity.Name = vm.Name;
            entity.Description = vm.Description;
            entity.Acronym = vm.Acronym;

            if (!string.IsNullOrWhiteSpace(vm.LogoUrl))
            {
                entity.LogoUrl = vm.LogoUrl;
            }

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