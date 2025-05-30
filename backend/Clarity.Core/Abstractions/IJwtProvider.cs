using Clarity.Core.Models;

namespace Clarity.Core.Abstractions;

public interface IJwtProvider
{
    Result<string> GenerateAuthToken(User user);
    Result<string> GenereateRefreshToken();
}