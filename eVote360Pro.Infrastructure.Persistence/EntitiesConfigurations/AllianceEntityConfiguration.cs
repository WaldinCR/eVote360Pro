using eVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Infrastructure.Persistence.EntitiesConfigurations
{
    public class AllianceEntityConfiguration : IEntityTypeConfiguration<Alliance>
    {
        public void Configure(EntityTypeBuilder<Alliance> builder)
        {
            #region Basic Configuration
            builder.ToTable("Alianzas");
            builder.HasKey(a => a.Id);
            #endregion

            #region Properties
            builder.Property(a => a.Party1Id).IsRequired();
            builder.Property(a => a.Party2Id).IsRequired();
            builder.Property(a => a.CreationDate).HasDefaultValueSql("GETDATE()"); 
            #endregion

            #region Relationships
            // Relación del Partido 1
            builder.HasOne(a => a.Party1)
                   .WithMany(p => p.AlliancesAsParty1)
                   .HasForeignKey(a => a.Party1Id)
                   .OnDelete(DeleteBehavior.Restrict); 

            // Relación del Partido 2
            builder.HasOne(a => a.Party2)
                   .WithMany(p => p.AlliancesAsParty2)
                   .HasForeignKey(a => a.Party2Id)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}