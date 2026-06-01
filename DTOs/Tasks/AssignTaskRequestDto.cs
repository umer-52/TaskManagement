using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs.Tasks;

/// <summary>
/// Admin (or owner) can reassign a task to another user by id.
/// </summary>
public class AssignTaskRequestDto
{
    [Required]
    public int UserId { get; set; }
}
