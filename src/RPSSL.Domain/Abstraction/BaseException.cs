using Microsoft.AspNetCore.Mvc;

namespace RPSSL.Domain.Abstraction;

public abstract class BaseException : Exception
{
    protected BaseException()
    { }

    protected BaseException(string message) : base(message)
    { }

    public abstract ProblemDetails ToProblemDetails();
}
