using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CertificateManager.Application.Exceptions;

[DefaultStatusCode(400)]
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    { }
}