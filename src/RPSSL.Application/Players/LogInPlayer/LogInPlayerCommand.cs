using RPSSL.Application.Abstractions.Messaging;

namespace RPSSL.Application.Players.LogInPlayer;

public sealed record LogInPlayerCommand(string Email, string Password) : ICommand<LogInPlayerResponse>;

