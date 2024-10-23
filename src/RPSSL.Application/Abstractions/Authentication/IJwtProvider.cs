using RPSSL.Domain.Players;

namespace RPSSL.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    string GenerateToken(Player Player);
}
