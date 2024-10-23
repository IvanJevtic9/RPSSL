using MediatR;
using RPSSL.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using RPSSL.Application.Players.LogInPlayer;
using RPSSL.Application.Players.RegisterPlayer;

namespace RPSSL.Api.Controllers.Users;

[ApiController]
[Route("api/players")]
public class PlayersController : ControllerBase
{
    private readonly ISender _sender;

    public PlayersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<ActionResult<Unit>> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new RegisterPlayerCommand(request.Email, request.Username, request.Password), cancellationToken);

        return result.ToResponse();
    }

    [HttpPost("login")]
    public async Task<ActionResult<LogInPlayerResponse>> LogIn([FromBody] LogInUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new LogInPlayerCommand(request.Email, request.Password), cancellationToken);

        return result.ToResponse();
    }
}
