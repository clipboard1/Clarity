using Clarity.Application.Users.Register;

namespace Clarity.IntegrationsTests.Application.Users;

public class RegisterCommandTests : BaseIntegrationTest
{
    private const string ValidUsername = "testuser";
    private const string ValidEmail = "testuser@example.com";
    private const string ValidPassword = "xxxtestpasswordxxx";
    
    public RegisterCommandTests(IntegrationTestWebAppFactory factory) : base(factory){}
    
    [Fact]
    public async Task Register_WithValidData_RegistersUser()
    {
        var command = new RegisterCommand(
            ValidUsername,
            ValidEmail,
            ValidPassword);

        var result = await CommandDispatcher.DispatchAsync<RegisterCommand>(command);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Register_WithEmptyUsername_ReturnsValidationError()
    {
        var command = new RegisterCommand(
            "",
            ValidEmail,
            ValidPassword);

        var result = await CommandDispatcher.DispatchAsync<RegisterCommand>(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "Username");
    }

    [Fact]
    public async Task Register_WithEmptyEmail_ReturnsValidationError()
    {
        var command = new RegisterCommand(
            ValidUsername,
            "",
            ValidPassword);

        var result = await CommandDispatcher.DispatchAsync<RegisterCommand>(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "Email");
    }

    [Fact]
    public async Task Register_WithEmptyPassword_ReturnsValidationError()
    {
        var command = new RegisterCommand(
            ValidUsername,
            ValidEmail,
            "");

        var result = await CommandDispatcher.DispatchAsync<RegisterCommand>(command);

        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Key == "Password");
    }
}