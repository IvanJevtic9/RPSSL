using RPSSL.Domain.Exceptions;
using RPSSL.Domain.Abstraction;
using RPSSL.Application.GameFlow.Shared;
using RPSSL.Application.Abstractions.Clients;
using RPSSL.Application.Abstractions.Messaging;

namespace RPSSL.Application.GameFlow.PickRandomChoice;

internal sealed class PickRandomChoiceHandler : IQueryHandler<PickRandomChoiceQuery, ChoiceResponse>
{
    private readonly IRandomNumberApiClient _client;

    public PickRandomChoiceHandler(IRandomNumberApiClient client)
    {
        _client = client;
    }

    public async Task<Result<ChoiceResponse>> Handle(PickRandomChoiceQuery request, CancellationToken cancellationToken)
    {
        return await _client.GetRandomChoiceAsync();
    }
}
