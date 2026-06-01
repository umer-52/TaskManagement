using TaskManagementApi.DTOs.Common;
using TaskManagementApi.DTOs.Tasks;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services;

/// <summary>
/// Task business rules: ownership checks, mapping entities ↔ DTOs.
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<TaskService> _logger;

    public TaskService(
        ITaskRepository taskRepository,
        IUserRepository userRepository,
        ILogger<TaskService> logger)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<PagedResultDto<TaskResponseDto>> GetTasksAsync(
        int currentUserId,
        bool isAdmin,
        TaskQueryParameters query)
    {
        var (items, totalCount) = await _taskRepository.GetPagedAsync(currentUserId, query, isAdmin);

        return new PagedResultDto<TaskResponseDto>
        {
            Items = items.Select(MapToDto).ToList(),
            Page = Math.Max(1, query.Page),
            PageSize = Math.Clamp(query.PageSize, 1, 50),
            TotalCount = totalCount
        };
    }

    public async Task<TaskResponseDto> GetByIdAsync(int id, int currentUserId, bool isAdmin)
    {
        var task = await _taskRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Task {id} not found.");

        EnsureCanAccess(task, currentUserId, isAdmin);
        return MapToDto(task);
    }

    public async Task<TaskResponseDto> CreateAsync(CreateTaskRequestDto request, int currentUserId)
    {
        var task = new TaskItem
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Priority = request.Priority,
            DueDate = request.DueDate,
            UserId = currentUserId,
            IsCompleted = false
        };

        await _taskRepository.AddAsync(task);
        task = await _taskRepository.GetByIdAsync(task.Id) ?? task;

        _logger.LogInformation("Task {TaskId} created by user {UserId}", task.Id, currentUserId);
        return MapToDto(task);
    }

    public async Task<TaskResponseDto> UpdateAsync(
        int id,
        UpdateTaskRequestDto request,
        int currentUserId,
        bool isAdmin)
    {
        var task = await _taskRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Task {id} not found.");

        EnsureCanAccess(task, currentUserId, isAdmin);

        task.Title = request.Title.Trim();
        task.Description = request.Description.Trim();
        task.Priority = request.Priority;
        task.DueDate = request.DueDate;
        task.IsCompleted = request.IsCompleted;

        await _taskRepository.UpdateAsync(task);
        return MapToDto(task);
    }

    public async Task DeleteAsync(int id, int currentUserId, bool isAdmin)
    {
        var task = await _taskRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Task {id} not found.");

        EnsureCanAccess(task, currentUserId, isAdmin);
        await _taskRepository.DeleteAsync(task);
        _logger.LogInformation("Task {TaskId} deleted", id);
    }

    public async Task<TaskResponseDto> AssignAsync(int taskId, AssignTaskRequestDto request, bool isAdmin)
    {
        if (!isAdmin)
            throw new UnauthorizedAccessException("Only admins can assign tasks to other users.");

        var task = await _taskRepository.GetByIdAsync(taskId)
            ?? throw new KeyNotFoundException($"Task {taskId} not found.");

        if (!await _userRepository.ExistsAsync(request.UserId))
            throw new KeyNotFoundException($"User {request.UserId} not found.");

        task.UserId = request.UserId;
        await _taskRepository.UpdateAsync(task);

        task = await _taskRepository.GetByIdAsync(taskId) ?? task;
        return MapToDto(task);
    }

    public async Task<TaskResponseDto> MarkCompleteAsync(int id, int currentUserId, bool isAdmin)
    {
        var task = await _taskRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Task {id} not found.");

        EnsureCanAccess(task, currentUserId, isAdmin);
        task.IsCompleted = true;
        await _taskRepository.UpdateAsync(task);
        return MapToDto(task);
    }

    private static void EnsureCanAccess(TaskItem task, int currentUserId, bool isAdmin)
    {
        if (!isAdmin && task.UserId != currentUserId)
            throw new UnauthorizedAccessException("You do not have permission to access this task.");
    }

    private static TaskResponseDto MapToDto(TaskItem task) => new()
    {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description,
        IsCompleted = task.IsCompleted,
        Priority = task.Priority.ToString(),
        DueDate = task.DueDate,
        UserId = task.UserId,
        AssignedUserName = task.User?.Name
    };
}
