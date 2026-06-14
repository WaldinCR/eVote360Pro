using eVote360Pro.Core.Application.Dtos.PoliticalLeaderAssignment;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.PoliticalLeaderAssignment;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces.Repositories;

namespace eVote360Pro.Core.Application.Services
{
    public class PoliticalLeaderAssignmentService : IPoliticalLeaderAssignmentService
    {
        private readonly IPoliticalLeaderAssignmentRepository _assignmentRepository;
        private readonly IPoliticalPartyRepository _partyRepository;

        public PoliticalLeaderAssignmentService(
            IPoliticalLeaderAssignmentRepository assignmentRepository,
            IPoliticalPartyRepository partyRepository)
        {
            _assignmentRepository = assignmentRepository;
            _partyRepository = partyRepository;
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

        public async Task AddAsync(SavePoliticalLeaderAssignmentViewModel vm)
        {
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

            await _assignmentRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _assignmentRepository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Asignación no encontrada.");
            await _assignmentRepository.DeleteAsync(entity);
        }

        public async Task<bool> CheckIfUserIsAssignedAsync(int userId)
        {
            var assignment = await _assignmentRepository.GetByUserIdAsync(userId);
            return assignment != null;
        }
    }
}