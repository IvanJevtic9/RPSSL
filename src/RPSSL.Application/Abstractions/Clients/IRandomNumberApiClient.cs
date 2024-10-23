using RPSSL.Application.GameFlow.Shared;

namespace RPSSL.Application.Abstractions.Clients;

public interface IRandomNumberApiClient
{
    Task<ChoiceResponse> GetRandomChoiceAsync();
}
