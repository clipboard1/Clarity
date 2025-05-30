using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Clarity.Core.Abstractions;
using Clarity.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Clarity.Infrastructure.Authentication;

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public Result<string> GenerateAuthToken(User user)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email)
                    ]),
                Expires = DateTime.UtcNow.AddMinutes(_options.ExpiresMinutes),
                SigningCredentials = credentials,
                Issuer = _options.Issuer,
                Audience = _options.Audience,
            };
            
            var handler = new JwtSecurityTokenHandler();
            
            string token = handler.WriteToken(handler.CreateToken(tokenDescriptor));

            return Result<string>.Success(token);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(Result.ToDict("General", ex.Message));
        }
    }

    public Result<string> GenereateRefreshToken()
    {
        return Result<string>.Success(Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))) ;
    }
}