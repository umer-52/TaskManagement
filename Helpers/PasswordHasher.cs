namespace TaskManagementApi.Helpers;

/// <summary>
/// Wraps BCrypt so password logic lives in one place.
/// BCrypt automatically salts hashes — industry standard for password storage.
/// </summary>
public static class PasswordHasher
{
    public static string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public static bool Verify(string password, string passwordHash)
        => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
