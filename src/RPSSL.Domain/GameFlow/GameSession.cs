using RPSSL.Domain.Abstraction;

namespace RPSSL.Domain.GameFlow;

public sealed class GameSession : Entity
{
    private GameSession() { }

    private GameSession(Guid? playerOneId, Guid? playerTwoId, GameType gameType = GameType.FirstTo1)
        : base(Guid.NewGuid())
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

    public ICollection<GameRound> GameRounds { get; private set; } = [];

    public static GameSession Create(Guid? playerOneId = null, Guid? playerTwoId = null, GameType gameType = GameType.FirstTo1) => new(playerOneId, playerTwoId, gameType);

    public override string ToString()
    {
        if (EndDate.HasValue)
        {
            return "Finished";
        }

        if (DateTime.UtcNow - StartDate > TimeSpan.FromHours(1))
        {
            return "Abandoned";
        }

        return "Active";
    }

    public GameRound PlayRound(Choice playerOneChoice, Choice playerTwoChoice)
    {
        if (EndDate.HasValue)
        {
            throw new InvalidOperationException("Game session is already over.");
        }

        var gameRound = GameRound.Create(Id, playerOneChoice, playerTwoChoice);

        GameRounds.Add(gameRound);

        return gameRound;
    }

    public void CompleteSession()
    {
        if (EndDate.HasValue)
        {
            throw new InvalidOperationException("Game session is already over.");
        }

        EndDate = DateTime.Now;
    }
}
