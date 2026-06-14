using eVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Infrastructure.Persistence.EntitiesConfigurations
{
    public class AllianceRequestEntityConfiguration : IEntityTypeConfiguration<AllianceRequest>
    {
        public void Configure(EntityTypeBuilder<AllianceRequest> builder)
        {
            #region Basic Configuration
            builder.ToTable("SolicitudesAlianzas");
            builder.HasKey(ar => ar.Id);
            #endregion

            #region Properties
            builder.Property(ar => ar.ApplicantPartyId).IsRequired();
            builder.Property(ar => ar.ReceiverPartyId).IsRequired();
            builder.Property(ar => ar.Status).IsRequired(); 
            builder.Property(ar => ar.RequestDate).HasDefaultValueSql("GETDATE()");
            builder.Property(ar => ar.ResponseDate).IsRequired(false); // Puede ser nulo porque está pendiente
            #endregion

            #region Relationships
            // Partido Solicitante
            builder.HasOne(ar => ar.ApplicantParty)
                   .WithMany(p => p.SentAllianceRequests)
                   .HasForeignKey(ar => ar.ApplicantPartyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Partido Receptor
            builder.HasOne(ar => ar.ReceiverParty)
                   .WithMany(p => p.ReceivedAllianceRequests)
                   .HasForeignKey(ar => ar.ReceiverPartyId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}