namespace TaskManagementApi.Models;

/// <summary>
/// Database entity for a task assigned to a user.
/// Named TaskItem to avoid clashing with System.Threading.Tasks.Task.
/// </summary>
public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } 
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    

    /// <summary>Foreign key — which user owns this task.</summary>
    public int UserId { get; set; }
    public User User { get; set; } = null!; 
} 
