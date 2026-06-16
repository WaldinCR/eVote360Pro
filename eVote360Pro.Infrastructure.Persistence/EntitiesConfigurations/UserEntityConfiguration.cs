using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Domain.Common.Enums;
using eVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Infrastructure.Persistence.EntitiesConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            #region Basic Configuration
            builder.ToTable("Usuarios");
            builder.HasKey(u => u.Id);
            #endregion

            #region Property Configuration
            builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(150);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.UserName).IsRequired().HasMaxLength(50);
            builder.HasIndex(u => u.UserName).IsUnique();
            builder.Property(u => u.Password).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Role).IsRequired().HasConversion<string>();
            builder.Property(u => u.IsActive).IsRequired().HasDefaultValue(true);
            #endregion

            #region agregacion de usuario administrador
            builder.HasData(
            new User
            {
                Id = 1,
                Name = "Administrador",
                LastName = "Sistema",
                Email = "admin@evote.com",
                UserName = "admin",
                Password = PasswordEncryptation.ComputeSha256Hash("Admin123*"),
                Role = UserRol.Administrador,
                IsActive = true
            }
            );
            #endregion
          
        }
    }
}
