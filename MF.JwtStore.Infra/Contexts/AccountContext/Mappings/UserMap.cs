using MF.JwtStore.Core.Contexts.AccountContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MF.JwtStore.Infra.Contexts.AccountContext.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User", "dbo");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(120)
            .IsRequired(true);

        builder.Property(e => e.Image)
            .HasColumnName("Image")
            .HasColumnType("VARCHAR")
            .HasMaxLength(255)
            .IsRequired(true);

        builder.OwnsOne(e => e.Email)
            .Property(e => e.Address)
            .HasColumnName("Email")
            .IsRequired(true);

        builder.OwnsOne(e => e.Email)
            .OwnsOne(e => e.Verification)
            .Property(e => e.Code)
            .HasColumnName("EmailVerificationCode")
            .IsRequired(true);

        builder.OwnsOne(e => e.Email)
            .OwnsOne(e => e.Verification)
            .Property(e => e.ExpiresAt)
            .HasColumnName("EmailVerificationExpiresAt")
            .IsRequired(false);

        builder.OwnsOne(e => e.Email)
            .OwnsOne(e => e.Verification)
            .Property(e => e.VerifiedAt)
            .HasColumnName("EmailVerificationVerifiedAt")
            .IsRequired(false);

        builder.OwnsOne(e => e.Email)
            .OwnsOne(e => e.Verification)
            .Ignore(e => e.IsActive);

        builder.OwnsOne(e => e.Password)
            .Property(e => e.Hash)
            .HasColumnName("PasswordHash")
            .IsRequired();

        builder.OwnsOne(e => e.Password)
            .Property(e => e.ResetCode)
            .HasColumnName("PasswordResetCode")
            .IsRequired();

        builder.HasMany(x => x.Roles)
               .WithMany(x => x.Users)
               .UsingEntity<Dictionary<string, object>>("RoleUser",
                     role => role.HasOne<Role>()
                                 .WithMany()
                                 .HasForeignKey("RolesId")
                                 .OnDelete(DeleteBehavior.Cascade),
                     user => user.HasOne<User>()
                                 .WithMany()
                                 .HasForeignKey("UsersId")
                                 .OnDelete(DeleteBehavior.Cascade));
    }
}