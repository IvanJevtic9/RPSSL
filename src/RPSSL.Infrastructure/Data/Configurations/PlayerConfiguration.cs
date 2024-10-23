using RPSSL.Domain.Players;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RPSSL.Infrastructure.Data.Configurations;

internal sealed class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("Players");

        builder.HasKey(player => player.Id);

        builder.Property(player => player.Email)
            .HasMaxLength(400)
            .HasConversion(email => email.Value, value => new Email(value));

        builder.Property(player => player.Username)
            .HasMaxLength(200)
            .HasConversion(username => username.Value, value => new Username(value));

        builder.Property(player => player.PasswordHash)
            .HasConversion(passwordHash => passwordHash.Value, value => new PasswordHash(value));

        builder.HasIndex(player => player.Email)
            .IsUnique();

        builder.HasIndex(player => player.Username)
            .IsUnique();
    }
}
