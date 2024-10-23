using RPSSL.Domain.GameFlow;

namespace RPSSL.Application.Game.Factories;

internal abstract class GameRoundState
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
            GameResult.Tie => Constants.GameOutcome.Tie,
            GameResult.PlayerOneWon when isCurrentPlayerOne => Constants.GameOutcome.Win,
            _ => Constants.GameOutcome.Lost,
        };
    }
}

internal sealed class PlayerOneWonGameRound : GameRoundState
{
    public PlayerOneWonGameRound(GameRound representation) : base(representation) { }

    public override GameResult Result => GameResult.PlayerOneWon;
}

internal sealed class PlayerTwoWonGameRound : GameRoundState
{
    public PlayerTwoWonGameRound(GameRound representation) : base(representation) { }

    public override GameResult Result => GameResult.PlayerTwoWon;
}

internal sealed class TiedGameRound : GameRoundState
{
    public TiedGameRound(GameRound representation) : base(representation) { }

    public override GameResult Result => GameResult.Tie;
}
