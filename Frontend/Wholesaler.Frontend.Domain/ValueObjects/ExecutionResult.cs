namespace Wholesaler.Frontend.Domain.ValueObjects;

public class ExecutionResult
{
    protected ExecutionResult(
        bool isSuccess,
        string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public bool IsSuccess { get; }
    public string Message { get; }

    public static ExecutionResult CreateSuccessful()
    {
        return new(true, null);
    }

    public static ExecutionResult CreateFailed(string message)
    {
        return new(false, message);
    }
}
