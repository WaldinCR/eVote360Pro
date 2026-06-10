using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Citizen;
using eVote360Pro.Core.Application.Dtos.Citizen;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Core.Domain.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Services
{
    public class CitizenService : ICitizenService
    {
        private readonly IGenericRepository<Citizen> _citizenRepository;
        private readonly IGenericRepository<Election> _electionRepository;
        private readonly IGenericRepository<CitizenVote> _citizenVoteRepository;

        public CitizenService(
            IGenericRepository<Citizen> citizenRepository,
            IGenericRepository<Election> electionRepository,
            IGenericRepository<CitizenVote> citizenVoteRepository)
        {
            _citizenRepository = citizenRepository;
            _electionRepository = electionRepository;
            _citizenVoteRepository = citizenVoteRepository;
        }

        public async Task<IReadOnlyList<CitizenDto>> GetAllAsync()
        {
            var citizens = await _citizenRepository.GetAllAsync();
            return citizens.Select(c => new CitizenDto
            {
                Id = c.Id,
                Document = c.IdentificationNumber, // Mapeo corregido
                FirstName = c.Name,                // Mapeo corregido
                LastName = c.LastName,
                Email = c.Email,
                IsActive = c.IsActive
            }).ToList().AsReadOnly();
        }

        public async Task<SaveCitizenViewModel?> GetByIdSaveViewModelAsync(int id)
        {
            var entity = await _citizenRepository.GetByIdAsync(id);
            if (entity == null) return null;

            var allVotes = await _citizenVoteRepository.GetAllAsync();
            bool hasVoted = allVotes.Any(v => v.CitizenId == id);

            return new SaveCitizenViewModel
            {
                Id = entity.Id,
                Document = entity.IdentificationNumber, // Mapeo corregido
                FirstName = entity.Name,                // Mapeo corregido
                LastName = entity.LastName,
                Email = entity.Email,
                IsActive = entity.IsActive,
                HasVoted = hasVoted
            };
        }

        public async Task<string?> AddAsync(SaveCitizenViewModel viewModel)
        {
            if (await IsElectionActiveAsync()) return "No se pueden registrar ciudadanos porque hay una elección activa.";
            
            if (await IsDocumentDuplicatedAsync(viewModel.Document, 0)) return "El número de documento ya está registrado.";
            if (await IsEmailDuplicatedAsync(viewModel.Email, 0)) return "El correo electrónico ya está registrado.";

            var entity = new Citizen
            {
                IdentificationNumber = viewModel.Document, // Mapeo corregido
                Name = viewModel.FirstName,                // Mapeo corregido
                LastName = viewModel.LastName,
                Email = viewModel.Email,
                IsActive = true
            };

            await _citizenRepository.AddAsync(entity);
            return null;
        }

        public async Task<string?> UpdateAsync(SaveCitizenViewModel viewModel)
        {
            if (await IsElectionActiveAsync()) return "No se pueden modificar ciudadanos porque hay una elección activa.";

            if (await IsDocumentDuplicatedAsync(viewModel.Document, viewModel.Id)) return "El número de documento ya está registrado.";
            if (await IsEmailDuplicatedAsync(viewModel.Email, viewModel.Id)) return "El correo electrónico ya está registrado.";

            var entity = await _citizenRepository.GetByIdAsync(viewModel.Id);
            if (entity == null) return "El ciudadano no existe.";

            var allVotes = await _citizenVoteRepository.GetAllAsync();
            bool hasVoted = allVotes.Any(v => v.CitizenId == viewModel.Id);

            if (hasVoted && entity.IdentificationNumber != viewModel.Document) // Mapeo corregido
            {
                return "No se puede modificar el número de documento de un ciudadano que ya ha votado.";
            }

            entity.IdentificationNumber = hasVoted ? entity.IdentificationNumber : viewModel.Document; // Mapeo corregido
            entity.Name = viewModel.FirstName; // Mapeo corregido
            entity.LastName = viewModel.LastName;
            entity.Email = viewModel.Email;
            entity.IsActive = viewModel.IsActive;

            await _citizenRepository.UpdateAsync(entity);
            return null;
        }

        public async Task<string?> DeleteLogicalAsync(int id)
        {
            if (await IsElectionActiveAsync()) return "No se puede cambiar el estado porque hay una elección activa.";

            var entity = await _citizenRepository.GetByIdAsync(id);
            if (entity == null) return "El ciudadano no existe.";

            entity.IsActive = !entity.IsActive;
            await _citizenRepository.UpdateAsync(entity);
            return null;
        }

        // --- MÉTODOS PRIVADOS DE VALIDACIÓN ---
        private async Task<bool> IsElectionActiveAsync()
        {
            var activeElections = await _electionRepository.GetAllAsync();
            return activeElections.Any(e => e.Status == ElectionStatus.Active);
        }

        private async Task<bool> IsDocumentDuplicatedAsync(string document, int excludeId)
        {
            var all = await _citizenRepository.GetAllAsync();
            return all.Any(c => c.Id != excludeId && c.IdentificationNumber == document); // Mapeo corregido
        }

        private async Task<bool> IsEmailDuplicatedAsync(string email, int excludeId)
        {
            var all = await _citizenRepository.GetAllAsync();
            return all.Any(c => c.Id != excludeId && c.Email.ToLower() == email.ToLower());
        }
    }
}