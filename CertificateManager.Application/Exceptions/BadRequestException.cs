using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Certificate.Application.Exceptions;

[DefaultStatusCode(400)]
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    { }
}