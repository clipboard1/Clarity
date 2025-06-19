using Clarity.Application.Users.Login;
using Clarity.Application.Users.Logout;
using Clarity.Core.Dto;

namespace Clarity.IntegrationsTests.Application.Users;

public class LogoutCommandTests : BaseIntegrationTest
{
    private const string ValidPassword = "xxxtestpasswordxxx";
    
    public LogoutCommandTests(IntegrationTestWebAppFactory factory) : base(factory){}
    
    [Fact]
    public async Task Logout_WithValidToken_RevokesToken()
    {
        var user = await DataSeeder.AddUser();

        var loginCommand = new LoginCommand(user.Email, ValidPassword);
        var loginResult = await CommandDispatcher.DispatchAsync<LoginCommand, AuthTokens>(loginCommand);

        var command = new LogoutCommand(loginResult.Value.RefreshToken);

        var result = await CommandDispatcher.DispatchAsync(command);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Logout_WithInvalidToken_ReturnsError()
    {
        var command = new LogoutCommand("invalid_token");

        var result = await CommandDispatcher.DispatchAsync(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "RefreshToken");
    }

}