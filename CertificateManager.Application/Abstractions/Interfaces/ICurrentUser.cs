namespace Certificate.Application.Abstractions.Interfaces;

public interface ICurrentUser
{
    public string Username { get; }
    public Guid UserId { get; }
}