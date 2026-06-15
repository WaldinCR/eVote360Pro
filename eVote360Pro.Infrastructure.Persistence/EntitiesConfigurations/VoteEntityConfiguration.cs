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
            builder.HasOne(v => v.Election)
                .WithMany(e => e.Votes)
               .HasForeignKey(v => v.ElectionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.ElectivePosition) 
                 .WithMany(ep => ep.Votes)
                 .HasForeignKey(v => v.ElectivePositionId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.Candidate)
                .WithMany()
                 .HasForeignKey(v => v.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
