using eVote360Pro.Core.Application.Dtos.PoliticalParty;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eVote360Pro.Core.Application.Interfaces
{
    public interface IPoliticalPartyService
    {
        Task<List<PoliticalPartyDto>> GetAllAsync();
        Task<SavePoliticalPartyDto?> GetByIdSaveDtoAsync(int id);
        Task<SavePoliticalPartyDto?> AddAsync(SavePoliticalPartyDto dto);
        Task UpdateAsync(SavePoliticalPartyDto dto);
        Task ChangeStatusAsync(int id);
    }
}