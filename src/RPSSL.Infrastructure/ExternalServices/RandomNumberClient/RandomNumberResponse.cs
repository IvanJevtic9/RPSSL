using System.Text.Json.Serialization;

namespace RPSSL.Infrastructure.ExternalServices.RandomNumberClient;

public class RandomNumberResponse
{
    [JsonPropertyName("random_number")]
    public int RandomNumber { get; init; }
}

