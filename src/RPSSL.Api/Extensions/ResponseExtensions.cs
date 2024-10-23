using RPSSL.Api.Models;
using RPSSL.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using RPSSL.Domain.Abstraction;

namespace RPSSL.Api.Extensions;

internal static class ResponseExtensions
{
    public static ActionResult<TResponse> ToResponse<TResponse>(this Result<TResponse> response)
    {
        return response.IsSuccess
            ? new OkObjectResult(response.Value)
            : HandleErrorResponse(response.Exception);
    }

    private static ErrorObjectResult HandleErrorResponse(BaseException exception)
    {
        var problemDetails = exception switch
        {
            ValidationException validationException => validationException.ToProblemDetails(),
            RpsslApiException rpsslApiException => rpsslApiException.ToProblemDetails(),
            RandomNumberApiException randomNumberApiException => randomNumberApiException.ToProblemDetails(),
            _ => throw new InvalidOperationException("Unknown exception type.")
        };

        return new ErrorObjectResult(problemDetails, problemDetails!.Status);
    }
}
