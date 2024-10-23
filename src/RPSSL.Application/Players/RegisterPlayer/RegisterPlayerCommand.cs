using MediatR;
using RPSSL.Application.Abstractions.Messaging;

namespace RPSSL.Application.Players.RegisterPlayer;

public sealed record RegisterPlayerCommand(string Email, string Username, string Password) : ICommand<Unit>;
