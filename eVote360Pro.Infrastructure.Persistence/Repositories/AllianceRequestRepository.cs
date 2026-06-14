using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Core.Domain.Interfaces.Repositories;
using eVote360Pro.Infrastructure.Persistence.Contexts;

namespace eVote360Pro.Infrastructure.Persistence.Repositories
{
    public class AllianceRequestRepository : GenericRepository<AllianceRequest>, IAllianceRequestRepository
    {
     
        public AllianceRequestRepository(eVote360ProDbContext dbContext) : base(dbContext)
        {
           
        }
    }
}