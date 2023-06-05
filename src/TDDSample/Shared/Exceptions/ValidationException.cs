using Microsoft.AspNetCore.Http;

namespace TDDSample.Shared.Exceptions;

public class ValidationException : CustomException
{
    public ValidationException(string message, Exception? innerException = null, params string[] errors)
        : base(message, StatusCodes.Status400BadRequest, innerException, errors) { }
}
