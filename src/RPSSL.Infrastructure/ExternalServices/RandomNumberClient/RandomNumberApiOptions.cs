using System.ComponentModel.DataAnnotations;

namespace RPSSL.Infrastructure.ExternalServices.RandomNumberClient;

public sealed class RandomNumberApiOptions
{
    public const string ConfigurationSection = "RandomNumberClient";

    [Required]
    public string Url { get; init; }
}
