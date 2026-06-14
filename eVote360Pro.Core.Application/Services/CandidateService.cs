using eVote360Pro.Core.Application.Dtos.Candidate;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Candidate;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces.Repositories;

namespace eVote360Pro.Core.Application.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IPoliticalPartyRepository _partyRepository;

        public CandidateService(ICandidateRepository candidateRepository, IPoliticalPartyRepository partyRepository)
        {
            _candidateRepository = candidateRepository;
            _partyRepository = partyRepository;
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

        public async Task AddAsync(SaveCandidateViewModel vm)
        {
            // ViewModel → SaveDto → Entity
            var dto = new SaveCandidateDto
            {
                Name = vm.Name,
                LastName = vm.LastName,
                PhotoUrl = vm.PhotoUrl,
                IsActive = true,
                PoliticalPartyId = vm.PoliticalPartyId
            };

            var entity = new Candidate
            {
                Name = dto.Name,
                LastName = dto.LastName,
                PhotoUrl = dto.PhotoUrl ?? string.Empty,
                IsActive = true,
                ParticipatedInElection = false,
                PoliticalPartyId = dto.PoliticalPartyId
            };

            await _candidateRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(SaveCandidateViewModel vm)
        {
            var entity = await _candidateRepository.GetByIdAsync(vm.Id);
            if (entity == null) throw new Exception("Candidato no encontrado.");

            if (entity.ParticipatedInElection)
                throw new Exception("No se puede editar un candidato que participó en una elección activa o finalizada.");

            // ViewModel → SaveDto → Entity
            var dto = new SaveCandidateDto
            {
                Id = vm.Id,
                Name = vm.Name,
                LastName = vm.LastName,
                PhotoUrl = vm.PhotoUrl,
                IsActive = vm.IsActive,
                PoliticalPartyId = vm.PoliticalPartyId
            };

            entity.Name = dto.Name;
            entity.LastName = dto.LastName;
            if (!string.IsNullOrEmpty(dto.PhotoUrl))
                entity.PhotoUrl = dto.PhotoUrl;

            await _candidateRepository.UpdateAsync(entity);
        }

        public async Task ChangeStatusAsync(int id, bool status)
        {
            var entity = await _candidateRepository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Candidato no encontrado.");

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