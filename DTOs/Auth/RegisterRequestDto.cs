using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs.Auth;

/// <summary>
/// Incoming data for user registration — validated before hitting the service layer.
/// </summary>
public class RegisterRequestDto
{
    [Required, MinLength(2), MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6), MaxLength(100)]
    public string Password { get; set; } = string.Empty;
}
