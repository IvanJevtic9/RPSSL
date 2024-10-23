using System.Net.Http.Json;
using RPSSL.Domain.GameFlow;
using RPSSL.Application.GameFlow.Shared;
using RPSSL.Application.Abstractions.Clients;

namespace RPSSL.Infrastructure.ExternalServices.RandomNumberClient;

public class RandomNumberApiClient : IRandomNumberApiClient
{
    private readonly HttpClient _httpClient;

    public RandomNumberApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ChoiceResponse> GetRandomChoiceAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<RandomNumberResponse>("random") ?? throw new InvalidOperationException("Failed to fetch the random number.");

        var choice = (response.RandomNumber - 1) % 5 + 1;
        var choiceEnum = (Choice)choice;
        var choiceName = choiceEnum.ToString().ToLower();

        return new ChoiceResponse((int)choiceEnum, choiceName);
    }
}
