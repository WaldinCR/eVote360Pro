using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Infrastructure.Persistence.EntitiesConfigurations
{
    public class PoliticalLeaderEntityConfiguration : IEntityTypeConfiguration<PoliticalLeader>
    {
        public void Configure(EntityTypeBuilder<PoliticalLeader> builder)
        {
            #region Basic Configuration
            builder.ToTable("AsignacionesDirigentes");
            builder.HasKey(pl => pl.Id);
            #endregion

            #region Property Configuration
            builder.Property(pl => pl.UserId).IsRequired();
            builder.Property(pl => pl.PoliticalPartyId).IsRequired();
            #endregion

            #region Relationships Configuration 
            builder.HasIndex(pl => pl.UserId).IsUnique();
            builder.HasIndex(pl => pl.PoliticalPartyId).IsUnique();
            #endregion
        }
    }
}
