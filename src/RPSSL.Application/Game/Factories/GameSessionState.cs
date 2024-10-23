using RPSSL.Domain.GameFlow;

namespace RPSSL.Application.Game.Factories;

internal abstract class GameSessionState
{
    protected GameSessionState(GameSession representation, List<GameRoundState>? gameRoundStates = null)
    {
        Representation = representation;

        if (gameRoundStates == null)
        {
            gameRoundStates = [];

            foreach (var round in Representation.GameRounds)
            {
                gameRoundStates.Add(GameRoundStateFactory.ToModel(round));
            }
        }

        RoundStates = gameRoundStates;
    }

    protected GameSession Representation { get; }

    protected ICollection<GameRoundState> RoundStates { get; }

    public Guid SessionId => Representation.Id;

    public Guid? PlayerOneId => Representation.PlayerOneId;

    public Guid? PlayerTwoId => Representation.PlayerTwoId;

    public GameType GameType => Representation.GameType;

    public DateTime StartDate => Representation.StartDate;

    public DateTime? EndDate => Representation.EndDate;

    public int PlayerOneRoundWins => RoundStates.Count(r => r is PlayerOneWonGameRound);

    public int PlayerTwoRoundWins => RoundStates.Count(r => r is PlayerTwoWonGameRound);

    public int TiedRounds => RoundStates.Count(r => r is TiedGameRound);

    public int TotalRounds => Representation.GameRounds.Count;

    public string Status => Representation.ToString();

    public IReadOnlyCollection<GameRoundState> Rounds => (IReadOnlyCollection<GameRoundState>)RoundStates;

    public abstract bool IsLive { get; }
}

internal sealed class LiveGameSession : GameSessionState
{
    private static readonly Dictionary<GameType, int> _roundsToWinByGameType = new()
    {
        { GameType.FirstTo1, 1 },
        { GameType.FirstTo3, 3 },
        { GameType.FirstTo5, 5 }
    };

    public LiveGameSession(GameSession representation, List<GameRoundState>? gameRoundStates = null)
        : base(representation, gameRoundStates)
    { }

    public GameSessionState PlayRound(Choice playerOneChoice, Choice playerTwoChoice)
    {
        if (Representation.IsTerminated())
        {
            return new TerminatedSession(Representation, [.. RoundStates]);
        }

        var gameRound = Representation.PlayRound(playerOneChoice, playerTwoChoice);

        RoundStates.Add(GameRoundStateFactory.ToModel(gameRound));

        if (PlayerOneRoundWins == _roundsToWinByGameType[GameType] ||
            PlayerTwoRoundWins == _roundsToWinByGameType[GameType])
        {
            Representation.CompleteSession();
            return new OverGameSession(Representation, [.. RoundStates]);
        }

        return this;
    }

    public override bool IsLive => true;
}

internal sealed class OverGameSession : GameSessionState
{
    public OverGameSession(GameSession representation, List<GameRoundState>? gameRoundStates = null)
        : base(representation, gameRoundStates)
    { }

    public override bool IsLive => false;
}

internal sealed class TerminatedSession : GameSessionState
{
    public TerminatedSession(GameSession representation, List<GameRoundState>? gameRoundStates = null)
        : base(representation, gameRoundStates)
    { }

    public override bool IsLive => false;
}
