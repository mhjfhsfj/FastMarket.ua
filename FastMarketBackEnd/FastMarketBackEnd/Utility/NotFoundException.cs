namespace FastMarketBackEnd.Utility;

public class NotFoundException : Exception
{
    public int StatusCode { get; }

    public NotFoundException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}