namespace IMS.Application.Modules.Auth.Interfaces;

public interface ICurrentUser
{
    Guid Id { get; }
    string? Email { get; }
}