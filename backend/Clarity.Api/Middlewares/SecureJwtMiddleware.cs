using Clarity.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace Clarity.Api.Middlewares;

public class SecureJwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtOptions _options;

    public SecureJwtMiddleware(IOptions<JwtOptions> options, RequestDelegate next)
    {
        _options = options.Value;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Cookies[_options.AuthCookieName];

        if (!string.IsNullOrEmpty(token))
            context.Request.Headers.Append("Authorization", $"Bearer {token}");
        
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Xss-Protection", "1");
        context.Response.Headers.Append("X-Frame-Options", "DENY");

        await _next(context);
    }
}