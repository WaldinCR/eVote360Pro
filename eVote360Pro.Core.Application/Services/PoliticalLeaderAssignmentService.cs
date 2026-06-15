using eVote360Pro.Core.Application.Dtos.PoliticalLeaderAssignment;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.PoliticalLeaderAssignment;
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
    public class PoliticalLeaderAssignmentService : GenericService<PoliticalLeaderAssignment, SavePoliticalLeaderAssignmentViewModel>, IPoliticalLeaderAssignmentService
    {
        private readonly IPoliticalLeaderAssignmentRepository _assignmentRepository;
        private readonly IPoliticalPartyRepository _partyRepository;
        private readonly IGenericRepository<Election> _electionRepository;
        private readonly IMapper _mapper;

        public PoliticalLeaderAssignmentService(
            IPoliticalLeaderAssignmentRepository assignmentRepository,
            IPoliticalPartyRepository partyRepository,
            IGenericRepository<Election> electionRepository,
            IMapper mapper) : base(assignmentRepository, mapper)
        {
            _assignmentRepository = assignmentRepository;
            _partyRepository = partyRepository;
            _electionRepository = electionRepository;
            _mapper = mapper;
        }

        public async Task<List<PoliticalLeaderAssignmentViewModel>> GetAllViewModel()
        {
            var assignments = await _assignmentRepository.GetAllAsync();
            var parties = await _partyRepository.GetAllAsync();

            // Entity → DTO → ViewModel
            var dtos = assignments.Select(a => new PoliticalLeaderAssignmentDto
            {
                Id = a.Id,
                UserId = a.UserId,
                UserName = string.Empty, // se resuelve en controller desde sesión
                PoliticalPartyId = a.PoliticalPartyId,
                PoliticalPartyName = parties.FirstOrDefault(p => p.Id == a.PoliticalPartyId)?.Name ?? "Desconocido"
            }).ToList();

            return dtos.Select(d => new PoliticalLeaderAssignmentViewModel
            {
                Id = d.Id,
                UserId = d.UserId,
                UserName = d.UserName,
                PoliticalPartyId = d.PoliticalPartyId,
                PoliticalPartyName = d.PoliticalPartyName
            }).ToList();
        }

        public override async Task<SavePoliticalLeaderAssignmentViewModel?> AddAsync(SavePoliticalLeaderAssignmentViewModel vm)
        {
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active))
                throw new Exception("No se pueden realizar asignaciones mientras haya una elección activa.");

            if (await CheckIfUserIsAssignedAsync(vm.UserId))
                throw new Exception("Este dirigente ya está asignado a un partido político.");

            var all = await _assignmentRepository.GetAllAsync();
            if (all.Any(a => a.PoliticalPartyId == vm.PoliticalPartyId))
                throw new Exception("Este partido ya tiene un dirigente asignado.");

            var party = await _partyRepository.GetByIdAsync(vm.PoliticalPartyId);
            if (party == null || !party.IsActive)
                throw new Exception("El partido seleccionado no existe o está inactivo.");

            // ViewModel → SaveDto → Entity
            var dto = new SavePoliticalLeaderAssignmentDto
            {
                UserId = vm.UserId,
                PoliticalPartyId = vm.PoliticalPartyId
            };

            var entity = new PoliticalLeaderAssignment
            {
                UserId = dto.UserId,
                PoliticalPartyId = dto.PoliticalPartyId
            };

            var savedEntity = await _assignmentRepository.AddAsync(entity);
            return new SavePoliticalLeaderAssignmentViewModel
            {
                Id = savedEntity.Id,
                UserId = savedEntity.UserId,
                PoliticalPartyId = savedEntity.PoliticalPartyId
            };
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active))
                throw new Exception("No se pueden eliminar asignaciones mientras haya una elección activa.");

            var entity = await _assignmentRepository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Asignación no encontrada.");
            await _assignmentRepository.DeleteAsync(entity);
            return true;
        }

        public async Task<bool> CheckIfUserIsAssignedAsync(int userId)
        {
            var assignment = await _assignmentRepository.GetByUserIdAsync(userId);
            return assignment != null;
        }
    }
}