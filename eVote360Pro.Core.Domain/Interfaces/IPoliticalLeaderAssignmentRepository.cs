using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories
{
    public interface IPoliticalLeaderAssignmentRepository : IGenericRepository<PoliticalLeaderAssignment>
    {
        Task<PoliticalLeaderAssignment?> GetByUserIdAsync(int userId);
    }
}