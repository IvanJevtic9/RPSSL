using FluentValidation;

namespace RPSSL.Application.GameFlow.PlayRoundAgainstComputer;

internal sealed class PlayRoundAgainstComputerCommandValidator : AbstractValidator<PlayRoundAgainstComputerCommand>
{
    public PlayRoundAgainstComputerCommandValidator()
    {
        RuleFor(c => c.ChoiceId).InclusiveBetween(1, 5);
    }
}
