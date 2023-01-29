namespace OpenHealthTrackerApi.Pipeline;

public class HttpAccessDeniedException : Exception
{
    public HttpAccessDeniedException()
    {
    }

    public HttpAccessDeniedException(string message)
        : base(message)
    {
    }

    public HttpAccessDeniedException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

public class HttpNotFoundExeption : Exception
{
    public HttpNotFoundExeption()
    {
    }

    public HttpNotFoundExeption(string message)
        : base(message)
    {
    }

    public HttpNotFoundExeption(string message, Exception inner)
        : base(message, inner)
    {
    }
}