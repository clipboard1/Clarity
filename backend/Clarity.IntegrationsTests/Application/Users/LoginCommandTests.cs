using Clarity.Application.Users.Login;
using Clarity.Core.Dto;

namespace Clarity.IntegrationsTests.Application.Users;

public class LoginCommandTests : BaseIntegrationTest
{
    public LoginCommandTests(IntegrationTestWebAppFactory factory) : base(factory){}
    
    private const string ValidEmail = "testuser@example.com";
    private const string ValidPassword = "xxxtestpasswordxxx";

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsTokens()
    {
        var user = await DataSeeder.AddUser();

        var command = new LoginCommand(
            user.Email,
            ValidPassword);

        var result = await CommandDispatcher.DispatchAsync<LoginCommand, AuthTokens>(command);

        Assert.True(result.IsSuccess);
        Assert.False(string.IsNullOrWhiteSpace(result.Value.AuthToken));
        Assert.False(string.IsNullOrWhiteSpace(result.Value.RefreshToken));
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsError()
    {
        var user = await DataSeeder.AddUser();

        var command = new LoginCommand(
            user.Email,
            "invalid_password");

        var result = await CommandDispatcher.DispatchAsync<LoginCommand, AuthTokens>(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "Password");
    }

}