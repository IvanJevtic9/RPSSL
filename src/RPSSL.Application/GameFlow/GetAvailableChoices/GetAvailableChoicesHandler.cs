using RPSSL.Domain.GameFlow;
using RPSSL.Domain.Abstraction;
using RPSSL.Application.GameFlow.Shared;
using RPSSL.Application.Abstractions.Messaging;

namespace RPSSL.Application.GameFlow.GetAvailableChoices;

internal sealed class GetAvailableChoicesHandler : IQueryHandler<GetAvailableChoicesQuery, IReadOnlyList<ChoiceResponse>>
{
    private static readonly IReadOnlyList<ChoiceResponse> _choices = Enum.GetValues(typeof(Choice))
        .Cast<Choice>()
        .Select(choice => new ChoiceResponse(
            (int)choice,
            choice.ToString().ToLower()))
        .ToList();

    public async Task<Result<IReadOnlyList<ChoiceResponse>>> Handle(GetAvailableChoicesQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new Result<IReadOnlyList<ChoiceResponse>>(_choices));
    }
}
