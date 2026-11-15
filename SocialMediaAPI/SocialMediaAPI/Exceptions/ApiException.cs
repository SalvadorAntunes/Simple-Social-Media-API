namespace SocialMediaAPI.Exceptions
{
    // Thrown by the services when a request cannot be fulfilled.
    // ApiExceptionHandler turns it into an HTTP response with the given status code.
    public abstract class ApiException(int statusCode, string message) : Exception(message)
    {
        public int StatusCode { get; } = statusCode;
    }

    public class NotFoundException(string message)
        : ApiException(StatusCodes.Status404NotFound, message);

    public class ConflictException(string message)
        : ApiException(StatusCodes.Status409Conflict, message);
}
