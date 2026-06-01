namespace TaskManagementApi.DTOs.Users;

/// <summary>
/// Safe user representation for admin endpoints — never includes PasswordHash.
/// </summary>
public class UserResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
