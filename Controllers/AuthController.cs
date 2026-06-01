using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs.Auth;
using TaskManagementApi.Helpers;
using TaskManagementApi.Interfaces;

namespace TaskManagementApi.Controllers;

/// <summary>
/// Public endpoints: register and login (no JWT required).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);
        return Ok(ApiResponse<AuthResponseDto>.Ok(result, "Registration successful"));
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(ApiResponse<AuthResponseDto>.Ok(result, "Login successful"));
    }
}
