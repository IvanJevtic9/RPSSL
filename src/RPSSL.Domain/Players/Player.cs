using RPSSL.Domain.Abstraction;

namespace RPSSL.Domain.Players;

public sealed class Player : Entity
{
    private Player() { }

    private Player(Email email, Username username, PasswordHash passwordHash) : base(Guid.NewGuid())
    {
        Email = email;
        Username = username;
        PasswordHash = passwordHash;
    }

    public Email Email { get; private set; }

    public Username Username { get; private set; }

    public PasswordHash PasswordHash { get; private set; }

    public static Player Create(Email email, Username username, PasswordHash passwordHash) => new(email, username, passwordHash);
}
