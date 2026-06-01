using TaskManagementApi.Helpers;
using TaskManagementApi.Models;

namespace TaskManagementApi.Data;

/// <summary>
/// Seeds a default admin account on first run (development learning convenience).
/// Email: admin@taskapi.local | Password: Admin123!
/// </summary>
public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.Users.Any())
            return;

        var admin = new User
        {
            Name = "System Admin",
            Email = "admin@taskapi.local",
            PasswordHash = PasswordHasher.Hash("Admin123!"),
            Role = UserRole.Admin,
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}
