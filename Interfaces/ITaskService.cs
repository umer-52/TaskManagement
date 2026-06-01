using TaskManagementApi.DTOs.Common;
using TaskManagementApi.DTOs.Tasks;

namespace TaskManagementApi.Interfaces;

public interface ITaskService
{
    Task<PagedResultDto<TaskResponseDto>> GetTasksAsync(int currentUserId, bool isAdmin, TaskQueryParameters query);
    Task<TaskResponseDto> GetByIdAsync(int id, int currentUserId, bool isAdmin);
    Task<TaskResponseDto> CreateAsync(CreateTaskRequestDto request, int currentUserId);
    Task<TaskResponseDto> UpdateAsync(int id, UpdateTaskRequestDto request, int currentUserId, bool isAdmin);
    Task DeleteAsync(int id, int currentUserId, bool isAdmin);
    Task<TaskResponseDto> AssignAsync(int taskId, AssignTaskRequestDto request, bool isAdmin);
    Task<TaskResponseDto> MarkCompleteAsync(int id, int currentUserId, bool isAdmin);
}
