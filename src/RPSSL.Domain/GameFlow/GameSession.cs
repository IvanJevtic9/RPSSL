using RPSSL.Domain.Abstraction;
using static RPSSL.Domain.Constants;

namespace RPSSL.Domain.GameFlow;

public sealed class GameSession : Entity
{
    private GameSession() { }

    private GameSession(Guid? playerOneId, Guid? playerTwoId, GameType gameType = GameType.FirstTo1) : base(Guid.NewGuid())
    {
        GameType = gameType;
        PlayerOneId = playerOneId;
        PlayerTwoId = playerTwoId;
        StartDate = DateTime.Now;
    }

    public Guid? PlayerOneId { get; private set; }

    public Guid? PlayerTwoId { get; private set; }

    public GameType GameType { get; private set; }

    public DateTime StartDate { get; private set; }

    public DateTime? EndDate { get; private set; }

    internal ICollection<GameRound> GameRounds { get; private set; } = [];

    public IReadOnlyCollection<GameRound> Rounds => (IReadOnlyCollection<GameRound>)GameRounds;

    public static GameSession Create(Guid? playerOneId = null, Guid? playerTwoId = null, GameType gameType = GameType.FirstTo1) =>
        new(playerOneId, playerTwoId, gameType);

    public bool IsFinished() => EndDate.HasValue;

    public bool IsTerminated() => DateTime.UtcNow - StartDate > TimeSpan.FromHours(1);

    public override string ToString()
    {
        if (IsFinished())
        {
            return GameSessionStatus.FinishedStatus;
        }

        if (IsTerminated())
        {
            return GameSessionStatus.TerminatedStatus;
        }

        return GameSessionStatus.ActiveStatus;
    }

    internal GameRound PlayRound(Choice playerOneChoice, Choice playerTwoChoice)
    {
        ValidateGameSession();

        var gameRound = GameRound.Create(Id, playerOneChoice, playerTwoChoice);

        GameRounds.Add(gameRound);

        return gameRound;
    }

    internal void CompleteSession()
    {
        if (IsFinished())
        {
            throw new InvalidOperationException("Game session is already over.");
        }

        EndDate = DateTime.Now;
    }

    private void ValidateGameSession()
    {
        if (IsFinished())
        {
            throw new InvalidOperationException("Game session is already over.");
        }

        if (IsTerminated())
        {
            throw new InvalidOperationException("Game session is terminated.");
        }
    }
}
