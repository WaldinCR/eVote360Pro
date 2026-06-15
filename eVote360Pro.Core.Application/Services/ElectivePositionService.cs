using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.ElectivePosition;
using eVote360Pro.Core.Application.Dtos.ElectivePosition;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Core.Domain.Common.Enums;
using AutoMapper;

namespace eVote360Pro.Core.Application.Services
{
    public class ElectivePositionService : IElectivePositionService
    {
        private readonly IElectivePositionRepository _positionRepository;
        private readonly IGenericRepository<Election> _electionRepository;
        private readonly IGenericRepository<Vote> _voteRepository; 
        private readonly IMapper _mapper;

        public ElectivePositionService(
            IElectivePositionRepository positionRepository, 
            IGenericRepository<Election> electionRepository,
            IGenericRepository<Vote> voteRepository,
            IMapper mapper)
        {
            _positionRepository = positionRepository;
            _electionRepository = electionRepository;
            _voteRepository = voteRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ElectivePositionDto>> GetAllAsync()
        {
            var positions = await _positionRepository.GetAllAsync();
            return _mapper.Map<List<ElectivePositionDto>>(positions).AsReadOnly();
        }

        public async Task<SaveElectivePositionViewModel?> GetByIdSaveViewModelAsync(int id)
        {
            var entity = await _positionRepository.GetByIdAsync(id);
            if (entity == null) return null;

            var allVotes = await _voteRepository.GetAllAsync();
            bool hasParticipated = allVotes.Any(v => v.ElectivePositionId == id);

            var viewModel = _mapper.Map<SaveElectivePositionViewModel>(entity);
            // Inyectamos la variable calculada
            viewModel.HasParticipated = hasParticipated;

            return viewModel;
        }

        public async Task<string?> AddAsync(SaveElectivePositionViewModel viewModel)
        {
            if (await IsElectionActiveAsync()) return "No se pueden crear puestos porque hay una elección activa.";
            if (await IsNameDuplicatedAsync(viewModel.Name!, 0)) return "El nombre del puesto ya existe.";

            var entity = _mapper.Map<ElectivePosition>(viewModel);

            await _positionRepository.AddAsync(entity);
            return null; 
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
            if (hasParticipated && entity.Name.Trim() != viewModel.Name!.Trim())
            {
                return "No se puede modificar el nombre de un puesto que ya ha participado en una elección.";
            }

            string originalName = entity.Name;

            _mapper.Map(viewModel, entity);

            if (hasParticipated)
            {
                entity.Name = originalName;
            }

            await _positionRepository.UpdateAsync(entity);
            return null;
        }

        public async Task<string?> DeleteLogicalAsync(int id)
        {
            if (await IsElectionActiveAsync()) return "No se puede cambiar el estado porque hay una elección activa.";

            var entity = await _positionRepository.GetByIdAsync(id);
            if (entity == null) return "El puesto no existe.";

            if (entity.IsActive && await _positionRepository.HasAssociatedCandidatesAsync(id))
            {
                return "No se puede inactivar este puesto porque tiene candidatos activos asignados. Elimine las asignaciones primero.";
            }

            if (!entity.IsActive)
            {
                var allPositions = await _positionRepository.GetAllAsync();
                bool nameTaken = allPositions.Any(p => p.Id != id && p.IsActive && p.Name.Trim().ToLower() == entity.Name.Trim().ToLower());
                if (nameTaken) return "Ya existe otro puesto activo con el mismo nombre.";
            }

            entity.IsActive = !entity.IsActive;
            
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
            var normalizedInput = name.Trim().ToLower();
            var allPositions = await _positionRepository.GetAllAsync();
            return allPositions.Any(p => 
                p.Id != excludeId && 
                p.Name.Trim().ToLower() == normalizedInput);
        }
    }
}