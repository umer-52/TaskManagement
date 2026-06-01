using TaskManagementApi.Models;

namespace TaskManagementApi.Interfaces;

/// <summary>
/// Creates JWT tokens from authenticated user claims.
/// </summary>
public interface ITokenService
{
    (string Token, DateTime ExpiresAt) CreateToken(User user);
}
