using System.Text;
using Clarity.Api.Middlewares;
using Clarity.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Clarity.Api.Extensions;

public static class AuthExtension
{
    public static void AddApiAuthentication(
        this IServiceCollection services,
        IOptions<JwtOptions> jwtOptions)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey))
                };
            });
    }
    
    public static IApplicationBuilder UseSecureJwt(this IApplicationBuilder builder) => 
        builder.UseMiddleware<SecureJwtMiddleware>();
}