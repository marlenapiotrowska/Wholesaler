namespace Wholesaler.Frontend.Domain.ValueObjects;

public sealed class ExecutionResultGeneric<TResult> : ExecutionResult
{
    private ExecutionResultGeneric(
        bool isSuccess,
        string message,
        TResult payload)
        : base(isSuccess, message)
    {
        Payload = payload;
    }

    public TResult Payload { get; }

    public static ExecutionResultGeneric<TResult> CreateSuccessful(TResult payload)
    {
        return new(true, null, payload);
    }

    public static ExecutionResultGeneric<TResult> CreateFailed(string message)
    {
        return new(false, message, default);
    }
}
