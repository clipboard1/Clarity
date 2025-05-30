namespace Clarity.Infrastructure.Authentication;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiresMinutes { get; set; } = 0;
    public string AuthCookieName { get; set; } = string.Empty;
    public string RefreshCookieName { get; set; } = string.Empty;
}