using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace RPSSL.Domain.Exceptions;

public sealed class RpsslApiException : Exception
{
    public RpsslApiException(string message, string? title = null, int? statusCodes = null) : base(message)
    {
        Title = title;
        HttpStatusCode = statusCodes ?? StatusCodes.Status400BadRequest;
    }

    public int HttpStatusCode { get; }

    public string? Title { get; }

    public ProblemDetails ToProblemDetails() => new()
    {
        Title = Title,
        Status = HttpStatusCode,
        Type = nameof(RpsslApiException),
        Detail = Message
    };
}
