using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RPSSL.Domain.Exceptions;

public sealed class RandomNumberApiException : Exception
{
    public RandomNumberApiException(string message) : base(message)
    { }

    public ProblemDetails ToProblemDetails() => new()
    {
        Title = "Random Number Api Error",
        Type = nameof(RandomNumberApiException),
        Status = StatusCodes.Status503ServiceUnavailable,
        Detail = Message
    };
}
