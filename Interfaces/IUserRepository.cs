using TaskManagementApi.Models;

namespace TaskManagementApi.Interfaces;

/// <summary>
/// Data-access contract for users. Repositories talk to EF/DbContext only.
/// </summary>
public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByIdTrackedAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<IReadOnlyList<User>> GetAllAsync();
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<bool> ExistsAsync(int id);
}
