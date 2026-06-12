using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.ViewModels.Election;
using eVote360Pro.Core.Application.Dtos.Election;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces;
using eVote360Pro.Core.Domain.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace eVote360Pro.Core.Application.Services
{
    public class ElectionService : IElectionService
    {
        private readonly IGenericRepository<Election> _electionRepository;
        private readonly IGenericRepository<ElectivePosition> _positionRepository;
        private readonly IGenericRepository<PoliticalParty> _partyRepository;
        private readonly IGenericRepository<CandidatePosition> _assignmentRepository;
        private readonly IGenericRepository<Vote> _voteRepository;
        private readonly IGenericRepository<Candidate> _candidateRepository;

        public ElectionService(
            IGenericRepository<Election> electionRepository,
            IGenericRepository<ElectivePosition> positionRepository,
            IGenericRepository<PoliticalParty> partyRepository,
            IGenericRepository<CandidatePosition> assignmentRepository,
            IGenericRepository<Vote> voteRepository,
            IGenericRepository<Candidate> candidateRepository)
        {
            _electionRepository = electionRepository;
            _positionRepository = positionRepository;
            _partyRepository = partyRepository;
            _assignmentRepository = assignmentRepository;
            _voteRepository = voteRepository;
            _candidateRepository = candidateRepository;
        }

        public async Task<IReadOnlyList<ElectionDto>> GetAllAsync()
        {
            var elections = await _electionRepository.GetAllAsync();
            return elections
                .OrderByDescending(e => e.Year)
                .Select(e => new ElectionDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Year = e.Year,
                    Status = e.Status
                }).ToList().AsReadOnly();
        }

        public async Task<string?> AddAsync(SaveElectionViewModel viewModel)
        {
            var entity = new Election
            {
                Name = viewModel.Name,
                Year = viewModel.Year,
                Status = ElectionStatus.Pending 
            };

            await _electionRepository.AddAsync(entity);
            return null;
        }

        public async Task<string?> ActivateElectionAsync(int id)
        {
            var elections = await _electionRepository.GetAllAsync();
            if (elections.Any(e => e.Status == ElectionStatus.Active && e.Id != id))
                return "Ya existe una elección activa. Debe finalizarla antes de activar una nueva.";

            var electionToActivate = elections.FirstOrDefault(e => e.Id == id);
            if (electionToActivate == null) return "La elección no existe.";
            if (electionToActivate.Status != ElectionStatus.Pending) return "Solo se pueden activar elecciones en estado Pendiente.";

            var allPositions = await _positionRepository.GetAllAsync();
            var activePositions = allPositions.Where(p => p.IsActive).ToList();
            
            var allParties = await _partyRepository.GetAllAsync();
            var activeParties = allParties.Where(p => p.IsActive).ToList();
            
            var allAssignments = await _assignmentRepository.GetAllAsync();

            if (!activePositions.Any()) return "No se puede activar: No hay puestos electivos configurados y activos.";
            if (activeParties.Count < 2) return "No se puede activar: Se requieren al menos 2 partidos políticos activos para iniciar una elección.";

            foreach (var position in activePositions)
            {
                foreach (var party in activeParties)
                {
                    bool hasCandidate = allAssignments.Any(a => a.ElectivePositionId == position.Id && a.PoliticalPartyId == party.Id);
                    if (!hasCandidate)
                        return $"Configuración incompleta: El partido '{party.Name}' no tiene ningún candidato asignado para el puesto de '{position.Name}'.";
                }
            }

            electionToActivate.Status = ElectionStatus.Active;
            electionToActivate.ActivationDate = DateTime.UtcNow; 
            await _electionRepository.UpdateAsync(electionToActivate);
            return null;
        }

        public async Task<string?> FinishElectionAsync(int id)
        {
            var election = await _electionRepository.GetByIdAsync(id);
            if (election == null) return "La elección no existe.";
            if (election.Status != ElectionStatus.Active) return "Solo se pueden finalizar elecciones activas.";

            election.Status = ElectionStatus.Finished;
            election.FinishedDate = DateTime.UtcNow; 
            await _electionRepository.UpdateAsync(election);
            return null;
        }

        public async Task<ElectionResultViewModel?> GetResultsAsync(int id)
        {
            var election = await _electionRepository.GetByIdAsync(id);
            if (election == null || election.Status != ElectionStatus.Finished) return null;

            var allVotes = await _voteRepository.GetAllAsync();
            var electionVotes = allVotes.Where(v => v.ElectionId == id).ToList();

            var allPositions = await _positionRepository.GetAllAsync();
            var allCandidates = await _candidateRepository.GetAllAsync();
            var allParties = await _partyRepository.GetAllAsync();
            var allAssignments = await _assignmentRepository.GetAllAsync();

            var resultVm = new ElectionResultViewModel
            {
                ElectionId = election.Id,
                ElectionName = election.Name,
                Year = election.Year
            };

            var positionIds = allAssignments.Select(a => a.ElectivePositionId).Distinct().ToList();

            foreach (var posId in positionIds)
            {
                var position = allPositions.FirstOrDefault(p => p.Id == posId);
                if (position == null) continue;

                var votesForPosition = electionVotes.Where(v => v.ElectivePositionId == posId).ToList();
                int totalVotes = votesForPosition.Count;

                var posResult = new PositionResultViewModel
                {
                    PositionName = position.Name,
                    TotalVotes = totalVotes
                };

                var candidateGroups = votesForPosition.GroupBy(v => v.CandidateId).ToList();

                foreach (var group in candidateGroups)
                {
                    int votesCount = group.Count();
                    double percentage = totalVotes == 0 ? 0 : Math.Round((double)votesCount / totalVotes * 100, 2);

                    if (group.Key == null)
                    {
                        posResult.Candidates.Add(new CandidateResultViewModel
                        {
                            CandidateName = "Ninguno",
                            PartyName = "N/A",
                            Votes = votesCount,
                            Percentage = percentage
                        });
                    }
                    else
                    {
                        var candidate = allCandidates.FirstOrDefault(c => c.Id == group.Key);
                        var party = candidate != null ? allParties.FirstOrDefault(p => p.Id == candidate.PoliticalPartyId) : null;

                        posResult.Candidates.Add(new CandidateResultViewModel
                        {
                            CandidateName = candidate != null ? $"{candidate.Name} {candidate.LastName}" : "Desconocido",
                            PartyName = party != null ? party.Name : "Desconocido",
                            Votes = votesCount,
                            Percentage = percentage
                        });
                    }
                }

                bool hasNingunoEntry = posResult.Candidates.Any(c => c.CandidateName == "Ninguno");
                if (!hasNingunoEntry)
                {
                    posResult.Candidates.Add(new CandidateResultViewModel
                    {
                        CandidateName = "Ninguno",
                        PartyName = "N/A",
                        Votes = 0,
                        Percentage = 0
                    });
                }

                // Ordenar por cantidad de votos (de mayor a menor)
                posResult.Candidates = posResult.Candidates.OrderByDescending(c => c.Votes).ToList();

                // Lógica de Ganador y Empates
                if (posResult.Candidates.Any())
                {
                    var topVotes = posResult.Candidates.First().Votes;
                    var topCandidates = posResult.Candidates.Where(c => c.Votes == topVotes).ToList();

                    if (topCandidates.Count > 1 && topVotes > 0)
                    {
                        posResult.IsTie = true;
                        posResult.WinnerName = "¡EMPATE! (" + string.Join(" vs ", topCandidates.Select(c => c.CandidateName)) + ")";
                    }
                    else if (topVotes == 0)
                    {
                        posResult.IsTie = false;
                        posResult.WinnerName = "Sin votos";
                    }
                    else
                    {
                        posResult.IsTie = false;
                        posResult.WinnerName = topCandidates.First().CandidateName;
                    }
                }
                else
                {
                    posResult.WinnerName = "Sin votos";
                }

                resultVm.Positions.Add(posResult);
            }

            return resultVm;
        }
    }
}