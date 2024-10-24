using RPSSL.Domain.GameFlow;

namespace RPSSL.Domain.Game;

public static class GameRoundStateFactory
{
    private static readonly Dictionary<Choice, IReadOnlyList<Choice>> _winningChoices = new()
    {
        { Choice.Rock, new List<Choice> { Choice.Scissors, Choice.Lizard } },
        { Choice.Paper, new List<Choice> { Choice.Rock, Choice.Spock } },
        { Choice.Scissors, new List<Choice> { Choice.Paper, Choice.Lizard } },
        { Choice.Lizard, new List<Choice> { Choice.Spock, Choice.Paper } },
        { Choice.Spock, new List<Choice> { Choice.Scissors, Choice.Rock } }
    };

    public static GameRoundState ToModel(GameRound gameRound)
    {
        if (gameRound.PlayerOneChoice == gameRound.PlayerTwoChoice)
        {
            return new TiedGameRound(gameRound);
        }

        return _winningChoices[gameRound.PlayerOneChoice].Contains(gameRound.PlayerTwoChoice)
            ? new PlayerOneWonGameRound(gameRound)
            : new PlayerTwoWonGameRound(gameRound);
    }
}
