using TaskManagementApi.Models;

namespace TaskManagementApi.DTOs.Common;

/// <summary>
/// Query string parameters for filtering, search, and pagination on GET /api/tasks.
/// </summary>
public class TaskQueryParameters
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public bool? IsCompleted { get; set; }
    public TaskPriority? Priority { get; set; }
}
