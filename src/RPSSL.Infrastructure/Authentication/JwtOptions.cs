using System.ComponentModel.DataAnnotations;

namespace RPSSL.Infrastructure.Authentication;

public sealed class JwtOptions
{
    public const string ConfigurationSection = "Jwt";

    [Required]
    public string Key { get; init; }

    [Required]
    public string Issuer { get; init; }

    [Required]
    public string Audience { get; init; }

    [Required]
    public int ExpiresInMinutes { get; init; }
}
