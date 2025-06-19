using Clarity.Application.Users.Login;
using Clarity.Application.Users.LoginByRefreshToken;
using Clarity.Core.Dto;

namespace Clarity.IntegrationsTests.Application.Users;

public class LoginByRefreshTokenCommandTests : BaseIntegrationTest
{
    private const string ValidPassword = "xxxtestpasswordxxx";
    
    public LoginByRefreshTokenCommandTests(IntegrationTestWebAppFactory factory) : base(factory){}
    
    [Fact]
    public async Task LoginByRefreshToken_WithValidToken_ReturnsTokens()
    {
        var user = await DataSeeder.AddUser();

        var loginCommand = new LoginCommand(user.Email, ValidPassword);
        var loginResult = await CommandDispatcher.DispatchAsync<LoginCommand, AuthTokens>(loginCommand);
        var refreshToken = loginResult.Value.RefreshToken;

        var command = new LoginByRefreshTokenCommand(refreshToken);

        var result = await CommandDispatcher.DispatchAsync<LoginByRefreshTokenCommand, AuthTokens>(command);

        Assert.True(result.IsSuccess);
        Assert.False(string.IsNullOrWhiteSpace(result.Value.AuthToken));
        Assert.False(string.IsNullOrWhiteSpace(result.Value.RefreshToken));
        Assert.NotEqual(refreshToken, result.Value.RefreshToken);
    }

    [Fact]
    public async Task LoginByRefreshToken_WithInvalidToken_ReturnsError()
    {
        var command = new LoginByRefreshTokenCommand("invalid_refresh_token");

        var result = await CommandDispatcher.DispatchAsync<LoginByRefreshTokenCommand, AuthTokens>(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "RefreshToken");
    }

}