using eVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evote360_Pro.Infrastructure.Persistence.EntityConfigurations
{
    public class ElectivePositionEntityConfiguration : IEntityTypeConfiguration<ElectivePosition>
    {
        public void Configure(EntityTypeBuilder<ElectivePosition> builder)
        {
            builder.ToTable("ElectivePositions");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(250);
        }
    }
}