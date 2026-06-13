using eVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace eVote360Pro.Infrastructure.Persistence.EntitiesConfigurations
{
    public class VerificationCodeEntityConfiguration : IEntityTypeConfiguration<VerificationCode>
    {
        public void Configure(EntityTypeBuilder<VerificationCode> builder)
        {
            #region Basic Configuration
            builder.ToTable("CodigosVerificacion");
            builder.HasKey(v => v.Id);  
            #endregion

            #region Property Configuration
            builder.Property(v => v.CitizenId).IsRequired();
            builder.Property(v => v.ElectionId).IsRequired();
            builder.Property(v => v.Code).IsRequired().HasMaxLength(6);
            builder.Property(v => v.GeneratedAt).IsRequired();
            builder.Property(v => v.ExpiresAt).IsRequired();
            builder.Property(v => v.IsUsed).IsRequired().HasDefaultValue(false);
            builder.HasIndex(v => new { v.CitizenId, v.ElectionId });
            #endregion

            #region Relationship Configuration
            // FKs 
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

