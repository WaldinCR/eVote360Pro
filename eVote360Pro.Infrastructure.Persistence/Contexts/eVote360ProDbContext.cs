using Microsoft.EntityFrameworkCore;
using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Infrastructure.Persistence.Contexts
{
    public class eVote360ProDbContext : DbContext
    {
        public eVote360ProDbContext(DbContextOptions<eVote360ProDbContext> options) : base(options) { }

        public DbSet<Citizen> Citizens { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CitizenVote> CitizenVotes { get; set; }
        public DbSet<ElectivePosition> ElectivePositions { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<CandidatePosition> CandidatePositions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(eVote360ProDbContext).Assembly);
        }
    }
}
