using eVote360Pro.Core.Domain.Entities;
using eVote360Pro.Infrastructure.Persistence.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Election> Elections { get; set; }
        public DbSet<ElectivePosition> ElectivePositions { get; set; }
        public DbSet<CandidatePosition> CandidatePositions { get; set; }
        public DbSet<PoliticalParty> PoliticalParties { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<PoliticalLeaderAssignment> PoliticalLeaderAssignments { get; set; }
        //public DbSet<PoliticalLeader> PoliticalLeaders { get; set; }
        public DbSet<AllianceRequest> AllianceRequests { get; set; }
        public DbSet<Alliance> Alliances { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           modelBuilder.ApplyConfigurationsFromAssembly(typeof(eVote360ProDbContext).Assembly);
            //waldin
            modelBuilder.ApplyConfiguration(new PoliticalPartyEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CandidateEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PoliticalLeaderAssignmentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AllianceEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AllianceRequestEntityConfiguration());
        }

    }
}
