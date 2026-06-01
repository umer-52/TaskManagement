using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs.Common;
using TaskManagementApi.DTOs.Tasks;
using TaskManagementApi.Helpers;
using TaskManagementApi.Interfaces;

namespace TaskManagementApi.Controllers;

/// <summary>
/// Task CRUD — requires JWT. Users see own tasks; Admins see all.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResultDto<TaskResponseDto>>>> GetAll(
        [FromQuery] TaskQueryParameters query)
    {
        var result = await _taskService.GetTasksAsync(GetUserId(), IsAdmin(), query);
        return Ok(ApiResponse<PagedResultDto<TaskResponseDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<TaskResponseDto>>> GetById(int id)
    {
        var result = await _taskService.GetByIdAsync(id, GetUserId(), IsAdmin());
        return Ok(ApiResponse<TaskResponseDto>.Ok(result));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<TaskResponseDto>>> Create([FromBody] CreateTaskRequestDto request)
    {
        var result = await _taskService.CreateAsync(request, GetUserId());
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<TaskResponseDto>.Ok(result, "Task created"));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<TaskResponseDto>>> Update(
        int id,
        [FromBody] UpdateTaskRequestDto request)
    {
        var result = await _taskService.UpdateAsync(id, request, GetUserId(), IsAdmin());
        return Ok(ApiResponse<TaskResponseDto>.Ok(result, "Task updated"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        await _taskService.DeleteAsync(id, GetUserId(), IsAdmin());
        return Ok(ApiResponse<object>.Ok(new { }, "Task deleted"));
    }

    [HttpPatch("{id:int}/complete")]
    public async Task<ActionResult<ApiResponse<TaskResponseDto>>> MarkComplete(int id)
    {
        var result = await _taskService.MarkCompleteAsync(id, GetUserId(), IsAdmin());
        return Ok(ApiResponse<TaskResponseDto>.Ok(result, "Task marked complete"));
    }

    [HttpPatch("{id:int}/assign")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<ApiResponse<TaskResponseDto>>> Assign(
        int id,
        [FromBody] AssignTaskRequestDto request)
    {
        var result = await _taskService.AssignAsync(id, request, IsAdmin());
        return Ok(ApiResponse<TaskResponseDto>.Ok(result, "Task assigned"));
    }

    private int GetUserId()
        => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("User id not found in token."));

    private bool IsAdmin() => User.IsInRole("Admin");
}
