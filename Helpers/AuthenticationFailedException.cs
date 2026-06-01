namespace TaskManagementApi.Helpers;

/// <summary>
/// Thrown when login fails — middleware maps this to HTTP 401 Unauthorized.
/// </summary>
public class AuthenticationFailedException : Exception
{
    public AuthenticationFailedException(string message) : base(message) { }
}
