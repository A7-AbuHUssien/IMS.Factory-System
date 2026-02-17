using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Auth.DTOs;
using IMS.Application.Modules.Auth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers.Auth;

[Route("api/auth/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService authService)
    {
        _auth = authService;
    }

    [HttpPost("register")]
    public async Task<ApiResponse<AuthResponseDto>> Register([FromBody] RegisterRequestDto dto)
    {
        return new ApiResponse<AuthResponseDto>(await _auth.Register(dto));
    }
}