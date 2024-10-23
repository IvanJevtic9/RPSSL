using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPSSL.Domain.Abstraction;

namespace RPSSL.Domain.Exceptions;

public sealed class RandomNumberApiException : BaseException
{
    public RandomNumberApiException(string message) : base(message)
    { }

    public override ProblemDetails ToProblemDetails() => new()
    {
        Title = "Random Number Api Error",
        Type = nameof(RandomNumberApiException),
        Status = StatusCodes.Status503ServiceUnavailable,
        Detail = Message
    };
}
