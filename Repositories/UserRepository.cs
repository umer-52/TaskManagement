using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories;

/// <summary>
/// Implements IUserRepository using EF Core async methods.
/// Controllers never call this directly — services do.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
        => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

    /// <summary>Tracked entity for updates (EF change tracking).</summary>
    public async Task<User?> GetByIdTrackedAsync(int id)
        => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower());

    public async Task<IReadOnlyList<User>> GetAllAsync()
        => await _context.Users.AsNoTracking().OrderBy(u => u.Name).ToListAsync();

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
        => await _context.Users.AnyAsync(u => u.Id == id);
}
