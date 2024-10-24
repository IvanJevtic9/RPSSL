using RPSSL.Domain.Players;
using RPSSL.Domain.GameFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static RPSSL.Infrastructure.Constants;

namespace RPSSL.Infrastructure.Data.Configurations;

internal sealed class GameSessionConfiguration : IEntityTypeConfiguration<GameSession>
{
    public void Configure(EntityTypeBuilder<GameSession> builder)
    {
        builder.ToTable(Database.GameSessionTable, Database.GameScheme);

        builder.HasKey(gameSession => gameSession.Id);

        builder.HasOne<Player>()
            .WithMany()
            .HasForeignKey(gameSession => gameSession.PlayerOneId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Player>()
            .WithMany()
            .HasForeignKey(gameSession => gameSession.PlayerTwoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.OwnsMany(x => x.Rounds, x =>
        {
            x.ToTable(Database.GameRoundTable, Database.GameScheme);
            x.HasKey(x => x.Id);
            x.WithOwner().HasForeignKey(x => x.GameSessionId);
        }).Metadata
        .SetPropertyAccessMode(PropertyAccessMode.Property);
    }
}
