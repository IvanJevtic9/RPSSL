using FluentValidation;

namespace RPSSL.Application.Players.LogInPlayer;

internal sealed class LogInPlayerCommandValidator : AbstractValidator<LogInPlayerCommand>
{
    public LogInPlayerCommandValidator()
    {
        RuleFor(c => c.Email).EmailAddress();

        RuleFor(c => c.Password).NotEmpty();
    }
}
