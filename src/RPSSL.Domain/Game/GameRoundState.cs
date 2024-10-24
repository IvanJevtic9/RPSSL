using RPSSL.Domain.GameFlow;
using static RPSSL.Domain.Constants;

namespace RPSSL.Domain.Game;

public abstract class GameRoundState
{
    protected GameRoundState(GameRound representation) => Representation = representation;

    protected GameRound Representation { get; }

    public Guid GameSessionId => Representation.GameSessionId;

    public Choice PlayerOneChoice => Representation.PlayerOneChoice;

    public Choice PlayerTwoChoice => Representation.PlayerTwoChoice;

    public DateTime PlayedDate => Representation.PlayedDate;

    public abstract GameResult Result { get; }

    public string GetOutcomeMessage(bool isCurrentPlayerOne)
    {
        return Result switch
        {
            GameResult.Tie => GameOutcome.Tie,
            GameResult.PlayerOneWon when isCurrentPlayerOne => GameOutcome.Win,
            _ => GameOutcome.Lost,
        };
    }
}

public sealed class PlayerOneWonGameRound : GameRoundState
{
    public PlayerOneWonGameRound(GameRound representation) : base(representation) { }

    public override GameResult Result => GameResult.PlayerOneWon;
}

public sealed class PlayerTwoWonGameRound : GameRoundState
{
    public PlayerTwoWonGameRound(GameRound representation) : base(representation) { }

    public override GameResult Result => GameResult.PlayerTwoWon;
}

public sealed class TiedGameRound : GameRoundState
{
    public TiedGameRound(GameRound representation) : base(representation) { }

    public override GameResult Result => GameResult.Tie;
}
