using System.Security.Claims;
using IMS.Application.Modules.Auth.Interfaces;
using Microsoft.AspNetCore.Http;

namespace IMS.Infrastructure.Auth;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _http;

    public CurrentUser(IHttpContextAccessor http)
    {
        _http = http;
    }

    public Guid Id =>
        Guid.Parse(
            _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not authenticated")
        );

    public string? Email =>
        _http.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
}