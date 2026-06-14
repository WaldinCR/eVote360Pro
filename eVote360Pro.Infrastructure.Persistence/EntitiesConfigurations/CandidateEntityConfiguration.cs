using eVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Infrastructure.Persistence.EntitiesConfigurations
{
    public class CandidateEntityConfiguration : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            #region Basic Configuration
         
            builder.ToTable("Candidatos");

            // Llave Primaria
            builder.HasKey(c => c.Id);
            #endregion

            #region Properties
         
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.LastName).IsRequired().HasMaxLength(100);
            builder.Property(c => c.PhotoUrl).IsRequired();

            // Valores por defecto
            builder.Property(c => c.IsActive).HasDefaultValue(true);
            builder.Property(c => c.ParticipatedInElection).HasDefaultValue(false);
            #endregion

            #region Relationships
           
            builder.HasOne(c => c.PoliticalParty)
                   .WithMany(p => p.Candidates)
                   .HasForeignKey(c => c.PoliticalPartyId)
                   .OnDelete(DeleteBehavior.Restrict); 
            #endregion
        }
    }
}