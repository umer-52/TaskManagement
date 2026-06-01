using TaskManagementApi.DTOs.Users;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto).ToList();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdTrackedAsync(id)
            ?? throw new KeyNotFoundException($"User {id} not found.");

        await _userRepository.DeleteAsync(user);
        _logger.LogWarning("Admin deleted user {UserId}", id);
    }

    public async Task<UserResponseDto> PromoteUserAsync(int id, UserRole role)
    {
        var user = await _userRepository.GetByIdTrackedAsync(id)
            ?? throw new KeyNotFoundException($"User {id} not found.");

        user.Role = role;
        await _userRepository.UpdateAsync(user);
        _logger.LogInformation("User {UserId} role changed to {Role}", id, role);

        return MapToDto(user);
    }

    private static UserResponseDto MapToDto(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Role = user.Role.ToString(),
        CreatedAt = user.CreatedAt
    };
}
