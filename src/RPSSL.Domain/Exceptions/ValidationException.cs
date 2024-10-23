using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RPSSL.Domain.Exceptions;

public sealed record ValidationError(string PropertyName, string ErrorMessage);

public sealed class ValidationException : Exception
{
    public ValidationException(IEnumerable<ValidationError> validationErrors)
    {
        Errors = validationErrors;
    }

    public IEnumerable<ValidationError> Errors { get; }

    public ProblemDetails ToProblemDetails()
    {
        var problemDetails = new ProblemDetails()
        {
            Title = "Validation errors",
            Status = StatusCodes.Status400BadRequest,
            Type = nameof(ValidationException),
            Detail = Message
        };

        if (Errors.Any())
        {
            problemDetails.Extensions["errors"] = Errors;
        }

        return problemDetails;
    }
}
