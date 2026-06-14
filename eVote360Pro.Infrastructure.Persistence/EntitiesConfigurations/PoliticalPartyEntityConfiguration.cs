using eVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Infrastructure.Persistence.EntitiesConfigurations
{
    public class PoliticalPartyEntityConfiguration : IEntityTypeConfiguration<PoliticalParty>
    {
        public void Configure(EntityTypeBuilder<PoliticalParty> builder)
        {
            #region Basic Configuration
       
            builder.ToTable("PartidosPoliticos");

            
            builder.HasKey(p => p.Id);
            #endregion

            #region Properties
          
            builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
            builder.Property(p => p.Acronym).IsRequired().HasMaxLength(50);
            builder.Property(p => p.LogoUrl).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(500); 

            // Valores por defecto
            builder.Property(p => p.IsActive).HasDefaultValue(true);
            #endregion

            #region Indexes (Reglas de Negocio)
          
            builder.HasIndex(p => p.Name).IsUnique();
            builder.HasIndex(p => p.Acronym).IsUnique();
            #endregion
        }
    }
}