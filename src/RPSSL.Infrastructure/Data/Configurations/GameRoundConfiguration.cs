using RPSSL.Domain.GameFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RPSSL.Infrastructure.Data.Configurations;

internal sealed class GameRoundConfiguration : IEntityTypeConfiguration<GameRound>
{
    public void Configure(EntityTypeBuilder<GameRound> builder)
    {
        builder.ToTable("GameRounds");

        builder.HasKey(gameRound => gameRound.Id);

        builder.HasOne<GameSession>()
            .WithMany(session => session.GameRounds)
            .HasForeignKey(gameRound => gameRound.GameSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
