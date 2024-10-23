using RPSSL.Domain.Exceptions;
using RPSSL.Domain.Abstraction;

namespace RPSSL.Application.Extensions;

public static class ResultExtensions
{
    public static TResponse ToValidationErrorResult<TResponse>(ValidationException validationException)
    {
        if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var resultType = typeof(TResponse).GetGenericArguments()[0];
            var resultInstance = Activator.CreateInstance(typeof(Result<>).MakeGenericType(resultType), validationException);
            return (TResponse)resultInstance;
        }

        throw new InvalidOperationException("TResponse must be of type Result<T>");
    }
}
