using Clarity.Application.Users.Login;
using Clarity.Application.Users.LogoutAllDevices;
using Clarity.Core.Dto;

namespace Clarity.IntegrationsTests.Application.Users;

public class LogoutAllDevicesCommandTests : BaseIntegrationTest
{
    private const string ValidPassword = "xxxtestpasswordxxx";
    
    public LogoutAllDevicesCommandTests(IntegrationTestWebAppFactory factory) : base(factory){}
    
    [Fact]
    public async Task LogoutAllDevices_WithValidUserId_RevokesAllTokens()
    {
        var user = await DataSeeder.AddUser();
        
        var loginCommand = new LoginCommand(user.Email, ValidPassword);
        var loginResult = await CommandDispatcher.DispatchAsync<LoginCommand, AuthTokens>(loginCommand);
        
        var command = new LogoutAllDevicesCommand(user.Id);

        var result = await CommandDispatcher.DispatchAsync(command);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task LogoutAllDevices_WithEmptyUserId_ReturnsValidationError()
    {
        var command = new LogoutAllDevicesCommand(Guid.Empty);

        var result = await CommandDispatcher.DispatchAsync(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "UserId");
    }
}