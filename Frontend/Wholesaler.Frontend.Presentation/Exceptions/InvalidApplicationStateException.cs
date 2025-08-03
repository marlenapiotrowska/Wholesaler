namespace Wholesaler.Frontend.Presentation.Exceptions;

internal class InvalidApplicationStateException : Exception
{
    public InvalidApplicationStateException(string message)
        : base(message)
    {
    }
}