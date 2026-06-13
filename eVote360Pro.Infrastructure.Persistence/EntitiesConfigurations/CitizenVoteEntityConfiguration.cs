using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using eVote360Pro.Core.Domain.Entities;

namespace eVote360Pro.Infrastructure.Persistence.EntitiesConfigurations
{
    public class CitizenVoteEntityConfiguration : IEntityTypeConfiguration<CitizenVote>
    {
        public void Configure(EntityTypeBuilder<CitizenVote> builder)
        {
            #region Basic Configuration
            builder.ToTable("VotosCiudadanos");
            builder.HasKey(v => v.Id);
            #endregion

            #region Property Configuration
            builder.Property(v => v.CitizenId).IsRequired();
            builder.Property(v => v.ElectionId).IsRequired();
            builder.Property(v => v.VotedAt).IsRequired();
            builder.HasIndex(v => new { v.CitizenId, v.ElectionId }).IsUnique();
            #endregion

            #region Relationship Configuration
             //FKs 
             builder.HasOne<Citizen>()
                .WithMany()
                .HasForeignKey(v => v.CitizenId)
                .OnDelete(DeleteBehavior.Restrict);

             builder.HasOne<Election>()
                .WithMany()
                .HasForeignKey(v => v.ElectionId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }

    }
}
