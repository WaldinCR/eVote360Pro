using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.ElectivePosition;
using eVote360Pro.Core.Application.Dtos.ElectivePosition;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Core.Domain.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Services
{
    public class ElectivePositionService : IElectivePositionService
    {
        private readonly IElectivePositionRepository _positionRepository;
        private readonly IGenericRepository<Election> _electionRepository;
        private readonly IGenericRepository<Vote> _voteRepository; 

        public ElectivePositionService(
            IElectivePositionRepository positionRepository, 
            IGenericRepository<Election> electionRepository,
            IGenericRepository<Vote> voteRepository)
        {
            _positionRepository = positionRepository;
            _electionRepository = electionRepository;
            _voteRepository = voteRepository;
        }

        public async Task<IReadOnlyList<ElectivePositionDto>> GetAllAsync()
        {
            var positions = await _positionRepository.GetAllAsync();
            return positions.Select(p => new ElectivePositionDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                IsActive = p.IsActive
            }).ToList().AsReadOnly();
        }

        public async Task<SaveElectivePositionViewModel?> GetByIdSaveViewModelAsync(int id)
        {
            var entity = await _positionRepository.GetByIdAsync(id);
            if (entity == null) return null;

            var allVotes = await _voteRepository.GetAllAsync();
            bool hasParticipated = allVotes.Any(v => v.ElectivePositionId == id);

            return new SaveElectivePositionViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive,
                HasParticipated = hasParticipated
            };
        }

        public async Task<string?> AddAsync(SaveElectivePositionViewModel viewModel)
        {
            if (await IsElectionActiveAsync()) return "No se pueden crear puestos porque hay una elección activa.";
            if (await IsNameDuplicatedAsync(viewModel.Name!, 0)) return "El nombre del puesto ya existe.";

            var entity = new ElectivePosition
            {
                Name = viewModel.Name!,
                Description = viewModel.Description!,
                IsActive = true
            };

            await _positionRepository.AddAsync(entity);
            return null; // Éxito (retorna null si no hay error)
        }

        public async Task<string?> UpdateAsync(SaveElectivePositionViewModel viewModel)
        {
            if (await IsElectionActiveAsync()) return "No se pueden modificar puestos porque hay una elección activa.";
            if (await IsNameDuplicatedAsync(viewModel.Name!, viewModel.Id)) return "El nombre del puesto ya existe.";

            var entity = await _positionRepository.GetByIdAsync(viewModel.Id);
            if (entity == null) return "El puesto no existe.";

            var allVotes = await _voteRepository.GetAllAsync();
            bool hasParticipated = allVotes.Any(v => v.ElectivePositionId == viewModel.Id);

            // Bloqueo de modificación de nombre si ya participó
            if (hasParticipated && entity.Name != viewModel.Name)
            {
                return "No se puede modificar el nombre de un puesto que ya ha participado en una elección.";
            }

            entity.Name = hasParticipated ? entity.Name : viewModel.Name!;
            entity.Description = viewModel.Description!;
            entity.IsActive = viewModel.IsActive;

            await _positionRepository.UpdateAsync(entity);
            return null;
        }

        public async Task<string?> DeleteLogicalAsync(int id)
        {
            if (await IsElectionActiveAsync()) return "No se puede cambiar el estado porque hay una elección activa.";

            var entity = await _positionRepository.GetByIdAsync(id);
            if (entity == null) return "El puesto no existe.";

            entity.IsActive = !entity.IsActive; // Toggle lógico (Eliminación lógica / Activación)
            
            await _positionRepository.UpdateAsync(entity);
            return null;
        }

        // Métodos privados de validación
        private async Task<bool> IsElectionActiveAsync()
        {
            var activeElections = await _electionRepository.GetAllAsync();
            return activeElections.Any(e => e.Status == ElectionStatus.Active);
        }

        private async Task<bool> IsNameDuplicatedAsync(string name, int excludeId)
        {
            var normalizedInput = name.Replace(" ", "").ToLower();
            var allPositions = await _positionRepository.GetAllAsync();
            
            return allPositions.Any(p => 
                p.Id != excludeId && 
                p.Name.Replace(" ", "").ToLower() == normalizedInput);
        }
    }
}