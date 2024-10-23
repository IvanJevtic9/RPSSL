using FluentValidation;

namespace RPSSL.Application.Players.RegisterPlayer;

internal sealed class RegisterPlayerCommandValidator : AbstractValidator<RegisterPlayerCommand>
{
    public RegisterPlayerCommandValidator()
    {
        RuleFor(c => c.Username).NotEmpty();

        RuleFor(c => c.Email).EmailAddress();

        RuleFor(c => c.Password).NotEmpty().MinimumLength(5);
    }
}
