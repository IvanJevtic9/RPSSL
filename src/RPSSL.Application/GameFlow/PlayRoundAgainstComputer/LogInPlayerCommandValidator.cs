using FluentValidation;
using RPSSL.Application.GameFlow.PlayRoundAgainstComputer;

namespace RPSSL.Application.Players.LogInPlayer;

internal sealed class PlayRoundAgainstComputerCommandValidator : AbstractValidator<PlayRoundAgainstComputerCommand>
{
    public PlayRoundAgainstComputerCommandValidator()
    {
        RuleFor(c => c.ChoiceId).InclusiveBetween(1, 5);
    }
}
