using RPSSL.Application.GameFlow.Shared;

namespace RPSSL.Application.Abstractions.Client;

public interface IRandomNumberApiClient
{
    Task<ChoiceResponse> GetRandomChoiceAsync();
}
