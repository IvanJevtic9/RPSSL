namespace RPSSL.Api.Controllers.Users;

public sealed record RegisterUserRequest(string Email, string Username, string Password);
