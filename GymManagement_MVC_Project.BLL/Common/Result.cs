namespace GymManagement_MVC_Project.BLL.Common;

public interface IResult
{
    bool IsSuccess { get; }
    bool IsFailure => !IsSuccess;
    string? Error { get; }
    string? ErrorKey { get; }
    ErrorType? ErrorType { get; }

}

public class Result : IResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }
    public ErrorType? ErrorType { get; }
    public string? ErrorKey { get; }

    protected Result(bool isSuccess, string? error = null, string? errorKey = null, ErrorType? errorType = null)
    {
        ResultGuard.Validate(isSuccess, error);

        IsSuccess = isSuccess;
        Error = error;
        ErrorType = errorType;
        ErrorKey = errorKey;
    }

    public static Result Success()
    {
        return new(true);
    }

    public static Result Failure(string error, ErrorType errorType, string? errorKey = null)
    {
        return new(false, error, errorKey, errorType);
    }
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string? error = null, string? errorKey = null, ErrorType? errorType = null) : base(isSuccess, error, errorKey, errorType)
    {
        ResultGuard.Validate(isSuccess, error);

        if (isSuccess && value is null)
            throw new InvalidOperationException("Success message mus have a value");

        Value = value;
    }

    public static Result<T> Success(T value)
    {
        return new(true, value);
    }

    public static new Result<T> Failure(string error, ErrorType errorType, string? errorKey = null)
    {
        return new(false, default, error, errorKey, errorType);
    }
}

file class ResultGuard
{
    public static void Validate(bool isSuccess, string? error)
    {
        if (isSuccess && !string.IsNullOrWhiteSpace(error))
            throw new InvalidOperationException("Success result cannot have an error.");
        if (!isSuccess && string.IsNullOrWhiteSpace(error))
            throw new InvalidOperationException("Fai1ure result must have an error.");
    }
}


public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden,
    Failure
}