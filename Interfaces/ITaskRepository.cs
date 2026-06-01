using TaskManagementApi.DTOs.Common;
using TaskManagementApi.Models;

namespace TaskManagementApi.Interfaces;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(int id);
    Task<(IReadOnlyList<TaskItem> Items, int TotalCount)> GetPagedAsync(
        int userId,
        TaskQueryParameters query,
        bool isAdmin);
    Task<TaskItem> AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(TaskItem task);
}
