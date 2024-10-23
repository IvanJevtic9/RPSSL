using RPSSL.Domain.Players;
using RPSSL.Domain.GameFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RPSSL.Infrastructure.Data.Configurations;

internal sealed class GameSessionConfiguration : IEntityTypeConfiguration<GameSession>
{
    public void Configure(EntityTypeBuilder<GameSession> builder)
    {
        builder.ToTable("GameSessions");

        builder.HasKey(gameSession => gameSession.Id);

        builder.HasOne<Player>()
            .WithMany()
            .HasForeignKey(gameSession => gameSession.PlayerOneId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Player>()
            .WithMany()
            .HasForeignKey(gameSession => gameSession.PlayerTwoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
