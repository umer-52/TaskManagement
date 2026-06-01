using System.ComponentModel.DataAnnotations;
using TaskManagementApi.Models;

namespace TaskManagementApi.DTOs.Users;

public class PromoteUserRequestDto
{
    [Required]
    public UserRole Role { get; set; }
}
