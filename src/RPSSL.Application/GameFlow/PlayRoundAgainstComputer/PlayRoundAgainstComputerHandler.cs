using RPSSL.Domain.GameFlow;
using RPSSL.Domain.Abstraction;
using Microsoft.AspNetCore.Http;
using RPSSL.Application.Extensions;
using RPSSL.Application.GameFlow.Shared;
using RPSSL.Application.Abstractions.Client;
using RPSSL.Application.Abstractions.Service;
using RPSSL.Application.Abstractions.Messaging;

namespace RPSSL.Application.GameFlow.PlayRoundAgainstComputer;

internal sealed class PlayRoundAgainstComputerHandler : ICommandHandler<PlayRoundAgainstComputerCommand, PlayerVsComputerRoundOutcomeResponse>
{
    private readonly IGameService _gameService;
    private readonly IRandomNumberApiClient _client;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PlayRoundAgainstComputerHandler(IGameService gameService, IRandomNumberApiClient client, IHttpContextAccessor httpContextAccessor)
    {
        _client = client;
        _gameService = gameService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<PlayerVsComputerRoundOutcomeResponse>> Handle(PlayRoundAgainstComputerCommand request, CancellationToken cancellationToken)
    {
        var computerChoice = await _client.GetRandomChoiceAsync();

        var userIdentifier = _httpContextAccessor.HttpContext?.User?.GetUserIdentifier();

        var result = await _gameService.PlayRoundVsComputer((Choice)request.ChoiceId, (Choice)computerChoice.Id, userIdentifier);

        return new Result<PlayerVsComputerRoundOutcomeResponse>(result);
    }
}
