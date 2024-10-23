namespace RPSSL.Domain.Players;

public sealed record Username(string Value)
{
    public const string DefaultComputerUsername = "Computer";
    public const string DefaultAnonymousUsername = "Anonymous player";

    public static string GetOrDefault(string? username, bool isComputer = false)
    {
        return username ?? (isComputer ? DefaultComputerUsername : DefaultAnonymousUsername);
    }
}

