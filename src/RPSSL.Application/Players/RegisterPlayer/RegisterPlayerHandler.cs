using MediatR;
using RPSSL.Domain.Players;
using RPSSL.Domain.Exceptions;
using RPSSL.Domain.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RPSSL.Application.Abstractions.Data;
using RPSSL.Application.Abstractions.Messaging;

namespace RPSSL.Application.Players.RegisterPlayer;

internal sealed class RegisterPlayerHandler : ICommandHandler<RegisterPlayerCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<Player> _passwordHasher;

    public RegisterPlayerHandler(IUnitOfWork unitOfWork, IPasswordHasher<Player> passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<Unit>> Handle(RegisterPlayerCommand request, CancellationToken cancellationToken)
    {
        if (await IsEmailRegistered(request.Email))
        {
            return new Result<Unit>(new RpsslApiException(
                "Email Already Registered",
                $"The Player with the email '{request.Email}' has already been registered.",
                StatusCodes.Status409Conflict));
        }

        if (await IsUsernameRegistered(request.Username))
        {
            return new Result<Unit>(new RpsslApiException(
                "Username Already Registered",
                $"The Player with the username '{request.Username}' has already been registered.",
                StatusCodes.Status409Conflict));
        }

        var hashValue = _passwordHasher.HashPassword(null!, request.Password);

        _unitOfWork.Players.Add(Player.Create(
            new Email(request.Email),
            new Username(request.Username),
            new PasswordHash(hashValue)));

        await _unitOfWork.CommitAsync(cancellationToken);

        return new Result<Unit>(Unit.Value);
    }

    private async Task<bool> IsEmailRegistered(string email)
    {
        return await _unitOfWork.Players.AnyAsync(x => x.Email == new Email(email));
    }

    private async Task<bool> IsUsernameRegistered(string username)
    {
        return await _unitOfWork.Players.AnyAsync(x => x.Username == new Username(username));
    }
}
