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
        try
        {
            var randomChoice = await _client.GetRandomChoiceAsync();

            return new Result<ChoiceResponse>(randomChoice);
        }
        catch (Exception ex)
        {
            return new Result<ChoiceResponse>(new RandomNumberApiException(ex.Message));
        }
    }
}
