using MediatR;
using RPSSL.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RPSSL.Application.GameFlow.Shared;
using RPSSL.Application.GameFlow.PickRandomChoice;
using RPSSL.Application.GameFlow.GetAvailableChoices;
using RPSSL.Application.GameFlow.PlayRoundAgainstComputer;
using RPSSL.Application.GameFlow.GetPlayHistoryDetails;

namespace RPSSL.Api.Controllers.Game;

[ApiController]
[Route("api/game")]
public class GameController : ControllerBase
{
    private readonly ISender _sender;

    public GameController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("choice")]
    public async Task<ActionResult<ChoiceResponse>> PickRandomChoice(CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new PickRandomChoiceQuery(), cancellationToken);

        return result.ToResponse();
    }

    [HttpGet("choices")]
    public async Task<ActionResult<IReadOnlyList<ChoiceResponse>>> GetAvailableChoices(CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAvailableChoicesQuery(), cancellationToken);

        return result.ToResponse();
    }

    [HttpPost("play")]
    public async Task<ActionResult<PlayerVsComputerRoundOutcomeResponse>> PlayGameAgainstComputer([FromBody] PlayRoundAgainstComputerRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new PlayRoundAgainstComputerCommand(request.ChoiceId), cancellationToken);

        return result.ToResponse();
    }

    [Authorize]
    [HttpGet("sessions")]
    public async Task<ActionResult<IReadOnlyList<GameSessionResponse>>> GetPlayHistory(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPlayHistoryQuery(), cancellationToken);

        return result.ToResponse();
    }
}
