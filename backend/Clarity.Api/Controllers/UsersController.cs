using System.IdentityModel.Tokens.Jwt;
using Asp.Versioning;
using Clarity.Api.Contracts.Users;
using Clarity.Api.Extensions;
using Clarity.Application.Abstractions;
using Clarity.Application.Users.Login;
using Clarity.Application.Users.LoginByRefreshToken;
using Clarity.Application.Users.Logout;
using Clarity.Application.Users.LogoutAllDevices;
using Clarity.Application.Users.Register;
using Clarity.Core.Dto;
using Clarity.Core.Models;
using Clarity.Infrastructure.Authentication;
using Clarity.Infrastructure.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Clarity.Api.Controllers;


[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UsersController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly JwtOptions _options;

    public UsersController(ICommandDispatcher commandDispatcher, IOptions<JwtOptions> options)
    {
        _commandDispatcher = commandDispatcher;
        _options = options.Value;
    }
    
    [HttpPost("register")]
    [ValidateModel<RegisterUserRequest>]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var regCommand = new RegisterCommand(
            request.Username,
            request.Email,
            request.Password);

        var registerResult = await _commandDispatcher.DispatchAsync(regCommand, cancellationToken);
        if (!registerResult.IsSuccess)
            return BadRequest(new ValidationProblemDetails(registerResult.Errors));
        
        return Created();
    }

    [HttpPost("login")]
    [ValidateModel<LoginUserRequest>]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var logCommand = new LoginCommand(
            request.Email,
            request.Password);

        var loginResult = await _commandDispatcher.DispatchAsync<LoginCommand, AuthTokens>(
            logCommand,
            cancellationToken);

        if (!loginResult.IsSuccess)
            return BadRequest(new ValidationProblemDetails(loginResult.Errors));
        
        HttpContext.Response.Cookies.Append(
            _options.AuthCookieName,
            loginResult.Value.AuthToken,
            new CookieOptions{ MaxAge = TimeSpan.FromMinutes(_options.ExpiresMinutes)});
        
        HttpContext.Response.Cookies.Append(
            _options.RefreshCookieName,
            loginResult.Value.RefreshToken,
            new CookieOptions{ MaxAge = TimeSpan.FromHours(12)});

        return Ok(new
        {
            authToken = loginResult.Value.AuthToken,
            refreshToken = loginResult.Value.RefreshToken
        });
    }
    
    [HttpPost("login-refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginByRefreshToken(CancellationToken cancellationToken = default)
    {
        var refreshToken = HttpContext.Request.Cookies[_options.RefreshCookieName];
        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest(new ValidationProblemDetails(
                Result.ToDict("RefreshToken", "No token provided")));
        
        var logCommand = new LoginByRefreshTokenCommand(Uri.UnescapeDataString(refreshToken));

        var loginResult = await _commandDispatcher.DispatchAsync<LoginByRefreshTokenCommand, AuthTokens>(
            logCommand,
            cancellationToken);

        if (!loginResult.IsSuccess)
            return BadRequest(new ValidationProblemDetails(loginResult.Errors));
        
        HttpContext.Response.Cookies.Append(
            _options.AuthCookieName,
            loginResult.Value.AuthToken,
            new CookieOptions{ MaxAge = TimeSpan.FromMinutes(_options.ExpiresMinutes)});
        
        HttpContext.Response.Cookies.Append(
            _options.RefreshCookieName,
            loginResult.Value.RefreshToken,
            new CookieOptions{ MaxAge = TimeSpan.FromHours(12)});

        return Ok(new
        {
            authToken = loginResult.Value.AuthToken,
            refreshToken = loginResult.Value.RefreshToken
        });
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken = default)
    {
        var refreshToken = HttpContext.Request.Cookies[_options.RefreshCookieName];
        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest(new ValidationProblemDetails(
                Result.ToDict("RefreshToken", "No token provided")));

        var logoutCommand = new LogoutCommand(refreshToken);
        
        var logoutResult = await _commandDispatcher.DispatchAsync(logoutCommand, cancellationToken);

        if (!logoutResult.IsSuccess)
            return BadRequest(new ValidationProblemDetails(logoutResult.Errors));
        
        HttpContext.Response.Cookies.Delete(_options.AuthCookieName);
        HttpContext.Response.Cookies.Delete(_options.RefreshCookieName);

        return Ok();
    }
    
    [HttpPost("logout-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<IActionResult> LogoutAll(CancellationToken cancellationToken = default)
    {
        var userId = this.GetCurrentUserId();
        if(userId == Guid.Empty)
            return BadRequest();

        var logoutCommand = new LogoutAllDevicesCommand(userId);
        
        var logoutResult = await _commandDispatcher.DispatchAsync(logoutCommand, cancellationToken);
        if (!logoutResult.IsSuccess)
            return BadRequest(new ValidationProblemDetails(logoutResult.Errors));
        
        HttpContext.Response.Cookies.Delete(_options.AuthCookieName);
        HttpContext.Response.Cookies.Delete(_options.RefreshCookieName);

        return Ok();
    }
}