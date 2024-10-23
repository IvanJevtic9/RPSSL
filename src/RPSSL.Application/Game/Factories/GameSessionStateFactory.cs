using RPSSL.Domain.GameFlow;

namespace RPSSL.Application.Game.Factories;

internal static class GameSessionStateFactory
{
    public static GameSessionState ToModel(GameSession gameSession)
    {
        return gameSession switch
        {
            _ when gameSession.IsTerminated() => new TerminatedSession(gameSession),
            _ when gameSession.IsFinished() => new OverGameSession(gameSession),
            _ => new LiveGameSession(gameSession)
        };
    }
}
