using RPSSL.Domain.Abstraction;

namespace RPSSL.Domain.GameFlow;

public sealed class GameRound : Entity
{
    private GameRound() { }

    private GameRound(Guid gameSessionId, Choice playerOneChoice, Choice playerTwoChoice) : base(Guid.NewGuid())
    {
        GameSessionId = gameSessionId;
        PlayerOneChoice = playerOneChoice;
        PlayerTwoChoice = playerTwoChoice;
        PlayedDate = DateTime.Now;
    }

    public Guid GameSessionId { get; private set; }

    public Choice PlayerOneChoice { get; private set; }

    public Choice PlayerTwoChoice { get; private set; }

    public DateTime PlayedDate { get; private set; }

    internal static GameRound Create(Guid gameSessionId, Choice playerOneChoice, Choice playerTwoChoice) => 
        new(gameSessionId, playerOneChoice, playerTwoChoice);
}
