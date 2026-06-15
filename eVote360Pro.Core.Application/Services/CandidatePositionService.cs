using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.CandidatePosition;
using eVote360Pro.Core.Application.Dtos.CandidatePosition;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Core.Domain.Common.Enums;

namespace eVote360Pro.Core.Application.Services
{
    public class CandidatePositionService : ICandidatePositionService
    {
        private readonly IGenericRepository<CandidatePosition> _candidatePositionRepository;
        private readonly IGenericRepository<Candidate> _candidateRepository;
        private readonly IGenericRepository<ElectivePosition> _positionRepository;
        private readonly IGenericRepository<Election> _electionRepository;
        private readonly IGenericRepository<Alliance> _allianceRepository;    
        private readonly IGenericRepository<PoliticalParty> _partyRepository;  

        public CandidatePositionService(
            IGenericRepository<CandidatePosition> candidatePositionRepository,
            IGenericRepository<Candidate> candidateRepository,
            IGenericRepository<ElectivePosition> positionRepository,
            IGenericRepository<Election> electionRepository,
            IGenericRepository<Alliance> allianceRepository,
            IGenericRepository<PoliticalParty> partyRepository)
        {
            _candidatePositionRepository = candidatePositionRepository;
            _candidateRepository = candidateRepository;
            _positionRepository = positionRepository;
            _electionRepository = electionRepository;
            _allianceRepository = allianceRepository;
            _partyRepository = partyRepository;
        }

        public async Task<IReadOnlyList<CandidatePositionDto>> GetAllByPartyAsync(int partyId)
        {
            var assignments = await _candidatePositionRepository.GetAllAsync();
            var candidates = await _candidateRepository.GetAllAsync();
            var positions = await _positionRepository.GetAllAsync();

            var partyAssignments = assignments.Where(a => a.PoliticalPartyId == partyId).ToList();

            var dtos = partyAssignments.Select(a => {
                var candidate = candidates.FirstOrDefault(c => c.Id == a.CandidateId);
                var position = positions.FirstOrDefault(p => p.Id == a.ElectivePositionId);

                return new CandidatePositionDto
                {
                    Id = a.Id,
                    CandidateId = a.CandidateId,
                    CandidateName = candidate != null ? $"{candidate.Name} {candidate.LastName}" : "Desconocido",
                    ElectivePositionId = a.ElectivePositionId,
                    ElectivePositionName = position != null ? position.Name : "Desconocido",
                    PoliticalPartyId = a.PoliticalPartyId,
                    IsAllied = candidate != null && candidate.PoliticalPartyId != partyId
                };
            }).ToList();

            return dtos.AsReadOnly();
        }

        public async Task<string?> AddAsync(SaveCandidatePositionViewModel viewModel)
        {
            if (await IsElectionActiveAsync()) return "No se pueden asignar candidatos porque hay una elección activa.";

            var allAssignments = await _candidatePositionRepository.GetAllAsync();
            var candidate = await _candidateRepository.GetByIdAsync(viewModel.CandidateId);
            
            if (candidate == null) return "El candidato seleccionado no existe.";
            if (!candidate.IsActive) return "El candidato seleccionado está inactivo.";

            // 1. Un candidato por puesto en el mismo partido
            bool positionTaken = allAssignments.Any(cp => cp.ElectivePositionId == viewModel.ElectivePositionId && cp.PoliticalPartyId == viewModel.PoliticalPartyId);
            if (positionTaken) return "Este partido ya tiene un candidato asignado para este puesto.";

            // 2. Un candidato no puede aspirar a más de un puesto en el mismo partido
            bool candidateAlreadyAssigned = allAssignments.Any(cp => cp.CandidateId == viewModel.CandidateId && cp.PoliticalPartyId == viewModel.PoliticalPartyId);
            if (candidateAlreadyAssigned) return "Este candidato ya está postulado a un puesto en este partido.";

            // 3. Lógica de ALIANZAS
            if (candidate.PoliticalPartyId != viewModel.PoliticalPartyId)
            {
                var originParty = await _partyRepository.GetByIdAsync(candidate.PoliticalPartyId);
                if (originParty == null || !originParty.IsActive)
                    return "El partido de origen del candidato aliado está inactivo o no existe.";

                var allAlliances = await _allianceRepository.GetAllAsync();
                bool allianceExists = allAlliances.Any(a =>
                    (a.Party1Id == viewModel.PoliticalPartyId && a.Party2Id == candidate.PoliticalPartyId) ||
                    (a.Party1Id == candidate.PoliticalPartyId && a.Party2Id == viewModel.PoliticalPartyId));

                if (!allianceExists)
                    return $"No existe una alianza política vigente entre los partidos involucrados. Primero debe establecerse una alianza aceptada.";

                var originAssignment = allAssignments.FirstOrDefault(cp => cp.CandidateId == viewModel.CandidateId && cp.PoliticalPartyId == candidate.PoliticalPartyId);

                if (originAssignment == null) 
                    return "El candidato aliado debe tener un puesto asignado en su partido de origen primero.";

                if (originAssignment.ElectivePositionId != viewModel.ElectivePositionId) 
                    return "El candidato aliado solo puede postularse al mismo puesto que tiene en su partido de origen.";
            }

            var entity = new CandidatePosition
            {
                Id = viewModel.Id,
                CandidateId = viewModel.CandidateId,
                ElectivePositionId = viewModel.ElectivePositionId,
                PoliticalPartyId = viewModel.PoliticalPartyId
            };

            await _candidatePositionRepository.AddAsync(entity);
            return null;
        }

        public async Task<string?> DeleteAsync(int id)
        {
            if (await IsElectionActiveAsync()) return "No se pueden eliminar asignaciones porque hay una elección activa.";

            var entity = await _candidatePositionRepository.GetByIdAsync(id);
            if (entity == null) return "La asignación no existe.";

            var allAssignments = await _candidatePositionRepository.GetAllAsync();
            var candidate = await _candidateRepository.GetByIdAsync(entity.CandidateId);

            if (candidate != null && entity.PoliticalPartyId == candidate.PoliticalPartyId)
            {
                bool hasAlliedAssignments = allAssignments.Any(cp =>
                    cp.CandidateId == entity.CandidateId &&
                    cp.PoliticalPartyId != candidate.PoliticalPartyId);

                if (hasAlliedAssignments)
                    return "No se puede eliminar esta asignación porque el candidato tiene asignaciones aliadas activas. Elimine primero las asignaciones aliadas.";
            }
            
            await _candidatePositionRepository.DeleteAsync(entity);
            return null;
        }

        private async Task<bool> IsElectionActiveAsync()
        {
            var activeElections = await _electionRepository.GetAllAsync();
            return activeElections.Any(e => e.Status == ElectionStatus.Active);
        }
    }
}