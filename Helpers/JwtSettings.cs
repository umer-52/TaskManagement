namespace TaskManagementApi.Helpers;

/// <summary>
/// Strongly-typed JWT options bound from appsettings.json.
/// Keeps secrets and token settings out of hard-coded strings.
/// </summary>
public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 60;
}
