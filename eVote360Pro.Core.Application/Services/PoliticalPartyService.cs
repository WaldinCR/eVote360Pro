using eVote360Pro.Core.Application.Dtos.PoliticalParty;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.PoliticalParty;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces.Repositories;

namespace eVote360Pro.Core.Application.Services
{
    public class PoliticalPartyService : IPoliticalPartyService
    {
        private readonly IPoliticalPartyRepository _partyRepository;

        public PoliticalPartyService(IPoliticalPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
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

        public async Task AddAsync(SavePoliticalPartyViewModel vm)
        {
            // Validar siglas únicas
            var all = await _partyRepository.GetAllAsync();

            if (all.Any(p => p.Acronym.ToLower() == vm.Acronym.ToLower()))
                throw new Exception($"Ya existe un partido con las siglas '{vm.Acronym}'.");

            if (all.Any(p => p.Name.ToLower() == vm.Name.ToLower()))
                throw new Exception($"Ya existe un partido con el nombre '{vm.Name}'.");

            var dto = new SavePoliticalPartyDto
            {
                Name = vm.Name,
                Description = vm.Description,
                Acronym = vm.Acronym,
                LogoUrl = vm.LogoUrl,
                IsActive = true
            };

            var entity = new PoliticalParty
            {
                Name = dto.Name,
                Description = dto.Description,
                Acronym = dto.Acronym,
                LogoUrl = dto.LogoUrl ?? string.Empty,
                IsActive = dto.IsActive
            };

            await _partyRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(SavePoliticalPartyViewModel vm)
        {
            var entity = await _partyRepository.GetByIdAsync(vm.Id);
            if (entity == null) throw new Exception("Partido no encontrado.");

            var all = await _partyRepository.GetAllAsync();

            if (all.Any(p => p.Acronym.ToLower() == vm.Acronym.ToLower() && p.Id != vm.Id))
                throw new Exception($"Ya existe un partido con las siglas '{vm.Acronym}'.");

            if (all.Any(p => p.Name.ToLower() == vm.Name.ToLower() && p.Id != vm.Id))
                throw new Exception($"Ya existe un partido con el nombre '{vm.Name}'.");

            entity.Name = vm.Name;
            entity.Description = vm.Description;
            entity.Acronym = vm.Acronym;

            // Solo actualizar logo si se envió uno nuevo
            if (!string.IsNullOrEmpty(vm.LogoUrl))
                entity.LogoUrl = vm.LogoUrl;

            await _partyRepository.UpdateAsync(entity);
        }

        public async Task ChangeStatusAsync(int id)
        {
            var entity = await _partyRepository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Partido no encontrado.");

            entity.IsActive = !entity.IsActive;
            await _partyRepository.UpdateAsync(entity);
        }
    }
}