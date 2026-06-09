using eVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace eVote360Pro.Infrastructure.Persistence.EntitiesConfigurations
{
    public class VoteEntityConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {

            #region Basic Configuration
            builder.ToTable("Votos");
            builder.HasKey(v => v.Id);
            #endregion

            #region Property Configuration
            builder.Property(v => v.ElectionId).IsRequired();
            builder.Property(v => v.ElectivePositionId).IsRequired();
            builder.Property(v => v.CandidateId).IsRequired(false);
            #endregion

            #region Relationship Configuration
            // FKs 
            // builder.HasOne<Eleccion>()
            //     .WithMany()
            //     .HasForeignKey(v => v.EleccionId)
            //     .OnDelete(DeleteBehavior.Restrict);

            // builder.HasOne<PuestoElectivo>()
            //     .WithMany()
            //     .HasForeignKey(v => v.PuestoElectivoId)
            //     .OnDelete(DeleteBehavior.Restrict);

            // builder.HasOne<Candidato>()
            //     .WithMany()
            //     .HasForeignKey(v => v.CandidatoId)
            //     .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
}
