using RPSSL.Domain.Players;
using RPSSL.Domain.Exceptions;
using RPSSL.Domain.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RPSSL.Application.Abstractions.Data;
using RPSSL.Application.Abstractions.Messaging;
using RPSSL.Application.Abstractions.Authentication;

namespace RPSSL.Application.Players.LogInPlayer;

internal sealed class LogInPlayerHandler : ICommandHandler<LogInPlayerCommand, LogInPlayerResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher<Player> _passwordHasher;

    public LogInPlayerHandler(IUnitOfWork unitOfWork, IPasswordHasher<Player> passwordHasher, IJwtProvider jwtProvider)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<LogInPlayerResponse>> Handle(LogInPlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _unitOfWork.Players.FirstOrDefaultAsync(x => x.Email == new Email(request.Email), cancellationToken);

        if (player is null)
        {
            return new Result<LogInPlayerResponse>(new RpsslApiException(
                "Invalid Credentials",
                "Invalid email or password.",
                StatusCodes.Status400BadRequest));
        }

        var result = _passwordHasher.VerifyHashedPassword(player, player.PasswordHash.Value, request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return new Result<LogInPlayerResponse>(new RpsslApiException(
                "Invalid Credentials",
                "Invalid email or password.",
                StatusCodes.Status400BadRequest));
        }

        var token = _jwtProvider.GenerateToken(player);

        return new Result<LogInPlayerResponse>(new LogInPlayerResponse(token));
    }
}
