namespace TaskManagementApi.Models;

/// <summary>
/// Defines what a user is allowed to do in the API.
/// Admin = manage users; User = manage own tasks.
/// </summary>
public enum UserRole
{
    User = 0,
    Admin = 1
}
