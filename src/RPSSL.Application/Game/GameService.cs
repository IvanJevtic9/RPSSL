using RPSSL.Domain.GameFlow;
using RPSSL.Application.Game.Factories;
using RPSSL.Application.GameFlow.Shared;
using RPSSL.Application.Abstractions.Data;
using RPSSL.Application.Abstractions.Service;

namespace RPSSL.Application.Game;

internal sealed class GameService : IGameService
{
    private readonly IUnitOfWork _unitOfWork;

    public GameService(IUnitOfWork unitOfOfWork)
    {
        _unitOfWork = unitOfOfWork;
    }

    public async Task<PlayerVsComputerRoundOutcomeResponse> PlayRoundVsComputer(Choice playerChoice, Choice computerChoice, Guid? playerId = null)
    {
        var gameSession = GameSession.Create(playerId);

        var gameSessionState = (LiveGameSession)GameSessionStateFactory.ToModel(gameSession);

        gameSessionState.PlayRound(playerChoice, computerChoice);

        if (playerId.HasValue)
        {
            await _unitOfWork.GameSessions.AddAsync(gameSession);
            await _unitOfWork.CommitAsync();
        }

        var gameRoundState = gameSessionState.Rounds.Last();

        return new PlayerVsComputerRoundOutcomeResponse(
            (int)playerChoice,
            (int)computerChoice,
            gameRoundState.GetOutcomeMessage(true));
    }
}
