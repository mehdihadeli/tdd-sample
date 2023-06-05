using Microsoft.AspNetCore.Http;

namespace TDDSample.Shared.Exceptions;

public class AppException : CustomException
{
    public AppException(
        string message,
        int statusCode = StatusCodes.Status400BadRequest,
        Exception? innerException = null
    )
        : base(message, statusCode, innerException) { }
}
