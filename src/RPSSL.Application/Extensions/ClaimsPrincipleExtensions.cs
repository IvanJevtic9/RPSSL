using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace RPSSL.Application.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserIdentifier(this ClaimsPrincipal claimsPrincipal)
    {
        var userIdString = claimsPrincipal?.FindFirst(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (Guid.TryParse(userIdString, out var userId))
        {
            return userId;
        }

        return null;
    }

    public static string? GetUsername(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal?.FindFirst(x => x.Type == JwtRegisteredClaimNames.Name)?.Value;
    }
}
