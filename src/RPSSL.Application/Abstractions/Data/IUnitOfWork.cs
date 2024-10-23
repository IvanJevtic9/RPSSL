using RPSSL.Domain.Players;
using RPSSL.Domain.GameFlow;
using Microsoft.EntityFrameworkCore;

namespace RPSSL.Application.Abstractions.Data;

public interface IUnitOfWork
{
    DbSet<Player> Players { get; }

    DbSet<GameSession> GameSessions { get; }

    DbSet<GameRound> GameRounds { get; }

    Task CommitAsync(CancellationToken cancellationToken = default);
}
