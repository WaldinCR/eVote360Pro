using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces.Repositories;
using eVote360Pro.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote360Pro.Infrastructure.Persistence.Repositories
{
    public class AllianceRepository : GenericRepository<Alliance>, IAllianceRepository
    {

        public AllianceRepository(eVote360ProDbContext dbContext) : base(dbContext)
        {
        
        }
    }
}