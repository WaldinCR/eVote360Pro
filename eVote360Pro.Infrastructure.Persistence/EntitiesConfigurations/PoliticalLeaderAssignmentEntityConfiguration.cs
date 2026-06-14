using eVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Infrastructure.Persistence.EntitiesConfigurations
{
    public class PoliticalLeaderAssignmentEntityConfiguration : IEntityTypeConfiguration<PoliticalLeaderAssignment>
    {
        public void Configure(EntityTypeBuilder<PoliticalLeaderAssignment> builder)
        {
            #region Basic Configuration
            builder.ToTable("AsignacionesDirigentes");
            builder.HasKey(a => a.Id);
            #endregion

            #region Properties
            builder.Property(a => a.UserId).IsRequired();
            builder.Property(a => a.PoliticalPartyId).IsRequired();
            #endregion

            #region Relationships & Indexes
            // Índices Únicos 
            builder.HasIndex(a => a.UserId).IsUnique();
            builder.HasIndex(a => a.PoliticalPartyId).IsUnique(); 

            // Relación 1 a 1 con el Partido Político
            builder.HasOne(a => a.PoliticalParty)
                   .WithOne(p => p.PoliticalLeaderAssignment)
                   .HasForeignKey<PoliticalLeaderAssignment>(a => a.PoliticalPartyId)
                   .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasOne(a => a.User)
                   .WithMany() // Suponiendo que el User no tiene una lista de asignaciones explícita
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}