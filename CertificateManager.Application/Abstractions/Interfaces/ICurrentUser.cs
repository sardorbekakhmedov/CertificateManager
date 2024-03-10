namespace CertificateManager.Application.Abstractions.Interfaces;

public interface ICurrentUser
{
    public string Username { get; }
    public Guid UserId { get; }
}