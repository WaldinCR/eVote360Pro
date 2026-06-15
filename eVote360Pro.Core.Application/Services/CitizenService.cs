using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Citizen;
using eVote360Pro.Core.Application.Dtos.Citizen;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Core.Domain.Common.Enums;
using AutoMapper;

namespace eVote360Pro.Core.Application.Services
{
    public class CitizenService : GenericService<Citizen, CitizenDto>, ICitizenService
    {
        private readonly IGenericRepository<Citizen> _citizenRepository;
        private readonly IGenericRepository<Election> _electionRepository;
        private readonly IGenericRepository<CitizenVote> _citizenVoteRepository;
        private readonly IMapper _mapper;

        public CitizenService(
            IGenericRepository<Citizen> citizenRepository,
            IGenericRepository<Election> electionRepository,
            IGenericRepository<CitizenVote> citizenVoteRepository,
            IMapper mapper) : base(citizenRepository, mapper)
        {
            _citizenRepository = citizenRepository;
            _electionRepository = electionRepository;
            _citizenVoteRepository = citizenVoteRepository;
            _mapper = mapper;
        }

        public new async Task<IReadOnlyList<CitizenDto>> GetAllAsync()
        {
            var citizens = await base.GetAllAsync();
            return citizens.AsReadOnly();
        }

        public async Task<SaveCitizenViewModel?> GetByIdSaveViewModelAsync(int id)
        {
            var entity = await _citizenRepository.GetByIdAsync(id);
            return _mapper.Map<SaveCitizenViewModel>(entity);
        }

        public async Task<string?> AddAsync(SaveCitizenViewModel viewModel)
        {
            if (await IsElectionActiveAsync()) return "No se pueden registrar ciudadanos porque hay una elección activa.";
            
            if (await IsDocumentDuplicatedAsync(viewModel.Document, 0)) return "El número de documento ya está registrado.";
            if (await IsEmailDuplicatedAsync(viewModel.Email, 0)) return "El correo electrónico ya está registrado.";

            var entity = _mapper.Map<Citizen>(viewModel);

            await _citizenRepository.AddAsync(entity);
            return null;
        }

        public async Task<string?> UpdateAsync(SaveCitizenViewModel viewModel)
        {
            if (await IsElectionActiveAsync()) return "No se pueden modificar ciudadanos porque hay una elección activa.";

            var entity = await _citizenRepository.GetByIdAsync(viewModel.Id);
            if (entity == null) return "El ciudadano no existe.";

            // Verificación de si el ciudadano ya votó
            bool hasVoted = await HasCitizenVotedAsync(entity.Id); 

            if (hasVoted && entity.IdentificationNumber.Trim() != viewModel.Document.Trim())
            {
                return "No se puede modificar el número de documento de un ciudadano que ya ha votado.";
            }

            if (await IsDocumentDuplicatedAsync(viewModel.Document, viewModel.Id)) return "El número de documento ya está registrado por otro ciudadano.";
            if (await IsEmailDuplicatedAsync(viewModel.Email, viewModel.Id)) return "El correo electrónico ya está registrado por otro ciudadano.";

            // Respaldamos el documento original por si ya votó
            string originalDocument = entity.IdentificationNumber;

            // Inyectamos los nuevos valores del viewModel a la entidad existente
            _mapper.Map(viewModel, entity);
            
            // Restauramos el documento si el ciudadano ya había votado
            if (hasVoted) 
            {
                entity.IdentificationNumber = originalDocument;
            }

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

        public async Task<bool> IsElectionActiveAsync()
        {
            var activeElections = await _electionRepository.GetAllAsync();
            return activeElections.Any(e => e.Status == ElectionStatus.Active);
        }

        private async Task<bool> IsDocumentDuplicatedAsync(string document, int excludeId)
        {
            var trimmedDocument = document.Trim(); 
            var all = await _citizenRepository.GetAllAsync();
            return all.Any(c => c.Id != excludeId && c.IdentificationNumber.Trim() == trimmedDocument);
        }

        private async Task<bool> IsEmailDuplicatedAsync(string email, int excludeId)
        {
            var all = await _citizenRepository.GetAllAsync();
            return all.Any(c => c.Id != excludeId && c.Email.ToLower() == email.ToLower());
        }

        private async Task<bool> HasCitizenVotedAsync(int citizenId)
        {
            var votes = await _citizenVoteRepository.GetAllAsync();
            return votes.Any(v => v.CitizenId == citizenId);
        }
    }
}