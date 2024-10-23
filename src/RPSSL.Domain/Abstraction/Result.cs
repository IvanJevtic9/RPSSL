namespace RPSSL.Domain.Abstraction;

public class Result
{
    public Result()
    { }

    public Result(Exception exception)
    {
        Exception = exception;
    }

    public bool IsFailure => Exception is not null;

    public bool IsSuccess => !IsFailure;

    public Exception Exception { get; }
}

public class Result<TValue> : Result
{
    public Result(TValue value)
    {
        Value = value;
    }

    public Result(Exception exception) : base(exception)
    { }

    public TValue Value { get; }
}
