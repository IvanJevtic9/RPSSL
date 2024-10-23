using RPSSL.Domain.Players;
using RPSSL.Domain.GameFlow;
using Microsoft.EntityFrameworkCore;
using RPSSL.Application.Abstractions.Data;

namespace RPSSL.Infrastructure.Data;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<Player> Players => Set<Player>();

    public DbSet<GameSession> GameSessions => Set<GameSession>();

    public DbSet<GameRound> GameRounds => Set<GameRound>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
    }
}
