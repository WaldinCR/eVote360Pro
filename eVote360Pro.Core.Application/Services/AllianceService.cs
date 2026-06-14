using eVote360Pro.Core.Application.Dtos.Alliance;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Alliance;
using eVote360Pro.Core.Domain.Interfaces.Repositories;

namespace eVote360Pro.Core.Application.Services
{
    public class AllianceService : IAllianceService
    {
        private readonly IAllianceRepository _allianceRepository;
        private readonly IPoliticalPartyRepository _partyRepository;

        public AllianceService(IAllianceRepository allianceRepository, IPoliticalPartyRepository partyRepository)
        {
            _allianceRepository = allianceRepository;
            _partyRepository = partyRepository;
        }

        public async Task<List<AllianceViewModel>> GetAllViewModel()
        {
            var alliances = await _allianceRepository.GetAllAsync();
            var parties = await _partyRepository.GetAllAsync();

            var dtos = alliances.Select(a => new AllianceDto
            {
                Id = a.Id,
                Party1Id = a.Party1Id,
                Party1Name = parties.FirstOrDefault(p => p.Id == a.Party1Id)?.Name ?? "Desconocido",
                Party2Id = a.Party2Id,
                Party2Name = parties.FirstOrDefault(p => p.Id == a.Party2Id)?.Name ?? "Desconocido",
                CreationDate = a.CreationDate.ToString("dd/MM/yyyy")
            }).ToList();

            return dtos.Select(d => new AllianceViewModel
            {
                Id = d.Id,
                Party1Id = d.Party1Id,
                Party1Name = d.Party1Name,
                Party2Id = d.Party2Id,
                Party2Name = d.Party2Name,
                CreationDate = DateTime.Parse(d.CreationDate)
            }).ToList();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _allianceRepository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Alianza no encontrada.");
            await _allianceRepository.DeleteAsync(entity);
        }

        public async Task<bool> HasActiveAllianceAsync(int party1Id, int party2Id)
        {
            var alliances = await _allianceRepository.GetAllAsync();
            return alliances.Any(a =>
                (a.Party1Id == party1Id && a.Party2Id == party2Id) ||
                (a.Party1Id == party2Id && a.Party2Id == party1Id));
        }
    }
}