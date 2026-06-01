using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs.Users;
using TaskManagementApi.Helpers;
using TaskManagementApi.Interfaces;

namespace TaskManagementApi.Controllers;

/// <summary>
/// Admin-only user management. Protected by "AdminOnly" authorization policy.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class AdminController : ControllerBase
{
    private readonly IUserService _userService;

    public AdminController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("users")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<UserResponseDto>>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(ApiResponse<IReadOnlyList<UserResponseDto>>.Ok(users));
    }

    [HttpDelete("users/{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteUser(int id)
    {
        await _userService.DeleteUserAsync(id);
        return Ok(ApiResponse<object>.Ok(new { }, "User deleted"));
    }

    [HttpPatch("users/{id:int}/promote")]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> PromoteUser(
        int id,
        [FromBody] PromoteUserRequestDto request)
    {
        var user = await _userService.PromoteUserAsync(id, request.Role);
        return Ok(ApiResponse<UserResponseDto>.Ok(user, "User role updated"));
    }
}
