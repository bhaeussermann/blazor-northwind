namespace Northwind.Client.Services;

internal sealed class ApiException : Exception
{
    public ApiException(string message)
        : base(message)
    { }
}
