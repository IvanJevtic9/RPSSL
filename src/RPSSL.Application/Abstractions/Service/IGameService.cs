using RPSSL.Domain.GameFlow;
using RPSSL.Application.GameFlow.Shared;

namespace RPSSL.Application.Abstractions.Service;

public interface IGameService
{
    public Task<PlayerVsComputerRoundOutcomeResponse> PlayRoundVsComputer(Choice playerChoice, Choice computerChoice, Guid? playerId = null);
}
