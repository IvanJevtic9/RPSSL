using RPSSL.Domain.GameFlow;

namespace RPSSL.Application.Game.Factories;

internal static class GameSessionStateFactory
{
    public static GameSessionState ToModel(GameSession gameSession)
    {
        if (gameSession.EndDate.HasValue)
        {
            return new OverGameSession(gameSession);
        }

        return new LiveGameSession(gameSession);
    }
}
