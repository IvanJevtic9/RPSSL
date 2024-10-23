using RPSSL.Application.GameFlow.Shared;
using RPSSL.Domain.Abstraction;

namespace RPSSL.Application.Abstractions.Clients;

public interface IRandomNumberApiClient
{
    Task<Result<ChoiceResponse>> GetRandomChoiceAsync();
}
