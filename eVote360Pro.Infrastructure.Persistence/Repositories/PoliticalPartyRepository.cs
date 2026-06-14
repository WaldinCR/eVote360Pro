using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces.Repositories;
using eVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Infrastructure.Persistence.Repositories
{
    public class PoliticalPartyRepository : GenericRepository<PoliticalParty>, IPoliticalPartyRepository
    {
        public PoliticalPartyRepository(eVote360ProDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PoliticalParty?> GetByAcronymAsync(string acronym)
        {
            return await _dbContext.Set<PoliticalParty>()
                .FirstOrDefaultAsync(p => p.Acronym == acronym);
        }
    }
}