using eVote360Pro.Core.Application.Dtos.Alliance;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Alliance;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Core.Domain.Interfaces.Repositories;
using eVote360Pro.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Services
{
    public class AllianceService : IAllianceService
    {
        private readonly IAllianceRepository _allianceRepository;
        private readonly IPoliticalPartyRepository _partyRepository;
        private readonly IGenericRepository<Election> _electionRepository;
        private readonly IGenericRepository<CandidatePosition> _candidatePositionRepository;
        private readonly IGenericRepository<Candidate> _candidateRepository;

        public AllianceService(
            IAllianceRepository allianceRepository,
            IPoliticalPartyRepository partyRepository,
            IGenericRepository<Election> electionRepository,
            IGenericRepository<CandidatePosition> candidatePositionRepository,
            IGenericRepository<Candidate> candidateRepository)
        {
            _allianceRepository = allianceRepository;
            _partyRepository = partyRepository;
            _electionRepository = electionRepository;
            _candidatePositionRepository = candidatePositionRepository;
            _candidateRepository = candidateRepository;
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
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active))
                throw new Exception("No se puede eliminar la alianza mientras haya una elección activa.");

            var entity = await _allianceRepository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Alianza no encontrada.");

            // Check for allied candidates assigned between the two parties
            var assignments = await _candidatePositionRepository.GetAllAsync();
            var candidates = await _candidateRepository.GetAllAsync();

            bool hasAlliedCandidates = assignments.Any(cp => {
                var candidate = candidates.FirstOrDefault(c => c.Id == cp.CandidateId);
                if (candidate == null) return false;
                
                return (candidate.PoliticalPartyId == entity.Party1Id && cp.PoliticalPartyId == entity.Party2Id) ||
                       (candidate.PoliticalPartyId == entity.Party2Id && cp.PoliticalPartyId == entity.Party1Id);
            });

            if (hasAlliedCandidates)
                throw new Exception("No se puede eliminar la alianza porque existen candidatos aliados asignados entre ambos partidos.");

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