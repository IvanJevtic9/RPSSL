using System.Net.Http.Json;
using RPSSL.Domain.GameFlow;
using RPSSL.Application.GameFlow.Shared;
using RPSSL.Application.Abstractions.Clients;
using RPSSL.Domain.Abstraction;
using RPSSL.Domain.Exceptions;

namespace RPSSL.Infrastructure.ExternalServices.RandomNumberClient;

public class RandomNumberApiClient : IRandomNumberApiClient
{
    private readonly HttpClient _httpClient;

    public RandomNumberApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<ChoiceResponse>> GetRandomChoiceAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<RandomNumberResponse>("random") ?? throw new InvalidOperationException("Failed to fetch the random number.");
            var choice = (response.RandomNumber - 1) % 5 + 1;
            var choiceEnum = (Choice)choice;
            var choiceName = choiceEnum.ToString().ToLower();

            return new Result<ChoiceResponse>(new ChoiceResponse((int)choiceEnum, choiceName));
        }
        catch (Exception ex)
        {
            return new Result<ChoiceResponse>(new RandomNumberApiException(ex.Message));
        }
    }
}
