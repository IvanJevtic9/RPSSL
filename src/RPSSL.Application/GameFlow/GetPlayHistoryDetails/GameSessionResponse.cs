namespace RPSSL.Application.GameFlow.GetPlayHistoryDetails;

public sealed class GameSessionResponse
{
    public Guid Id { get; init; }

    public string PlayerOne { get; init; }

    public string PlayerTwo { get; init; }

    public string GameType { get; init; }

    public DateTime StartDate { get; init; }

    public DateTime? EndDate { get; init; }

    public int PlayerOneTotalRoundWins { get; init; }

    public int PlayerTwoTotalRoundWins { get; init; }

    public string Status { get; init; }

    public string Scoreboard => $"{PlayerOneTotalRoundWins} : {PlayerTwoTotalRoundWins}";

    public IReadOnlyList<GameRoundResponse> GameRounds { get; init; }
}

public sealed class GameRoundResponse
{
    public Guid GameSessionId { get; init; }

    public string PlayerOneChoice { get; init; }

    public string PlayerTwoChoice { get; init; }

    public string Result { get; init; }

    public DateTime PlayedDate { get; init; }
}
