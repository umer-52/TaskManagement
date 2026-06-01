namespace TaskManagementApi.Models;

/// <summary>
/// Database entity representing an application user.
/// Never expose PasswordHash to API clients — use DTOs instead.
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber {get; set; } = string.Empty;

    /// <summary>BCrypt hash — never store plain-text passwords.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.User;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation: one user can own many tasks
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
