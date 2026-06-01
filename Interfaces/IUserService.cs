using TaskManagementApi.DTOs.Users;
using TaskManagementApi.Models;

namespace TaskManagementApi.Interfaces;

public interface IUserService
{
    Task<IReadOnlyList<UserResponseDto>> GetAllUsersAsync();
    Task DeleteUserAsync(int id);
    Task<UserResponseDto> PromoteUserAsync(int id, UserRole role);
}
