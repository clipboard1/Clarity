using System.IdentityModel.Tokens.Jwt;
using Clarity.Api.Contracts.Users;
using Clarity.Api.Extensions;
using Clarity.Application.Abstractions;
using Clarity.Application.LogoutAllDevices;
using Clarity.Application.Users.Login;
using Clarity.Application.Users.LoginByRefreshToken;
using Clarity.Application.Users.Logout;
using Clarity.Application.Users.Register;
using Clarity.Core.Dto;
using Clarity.Core.Models;
using Clarity.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Clarity.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var regCommand = new RegisterCommand(
            request.Username,
            request.Email,
            request.Password);

        var registerResult = await _commandDispatcher.DispatchAsync(regCommand);
        if (!registerResult.IsSuccess)
        {
            var problemDetails = new ValidationProblemDetails(registerResult.Errors);
            return BadRequest(problemDetails);
        }
        
        return Created();
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var logCommand = new LoginCommand(
            request.Email,
            request.Password);

        var loginResult = await _commandDispatcher.DispatchAsync<LoginCommand, AuthTokens>(logCommand);

        if (!loginResult.IsSuccess)
        {
            var problemDetails = new ValidationProblemDetails(loginResult.Errors);
            return BadRequest(problemDetails);
        }
        
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
    public async Task<IActionResult> LoginByRefreshToken(string token)
    {
        var logCommand = new LoginByRefreshTokenCommand(Uri.UnescapeDataString(token));

        var loginResult = await _commandDispatcher.DispatchAsync<LoginByRefreshTokenCommand, AuthTokens>(logCommand);

        if (!loginResult.IsSuccess)
        {
            var problemDetails = new ValidationProblemDetails(loginResult.Errors);
            return BadRequest(problemDetails);
        }
        
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
    public async Task<IActionResult> Logout()
    {
        var refreshToken = HttpContext.Request.Cookies[_options.RefreshCookieName];
        if (string.IsNullOrEmpty(refreshToken))
        {
            var problemDetails = new ValidationProblemDetails(
                Result.ToDict("RefreshToken", "No token provided"));
            return BadRequest(problemDetails);
        }

        var logoutCommand = new LogoutCommand(refreshToken);
        
        var logoutResult = await _commandDispatcher.DispatchAsync(logoutCommand);

        if (!logoutResult.IsSuccess)
        {
            var problemDetails = new ValidationProblemDetails(logoutResult.Errors);
            return BadRequest(problemDetails);
        }
        
        HttpContext.Response.Cookies.Delete(_options.AuthCookieName);
        HttpContext.Response.Cookies.Delete(_options.RefreshCookieName);

        return Ok();
    }
    
    [HttpPost("logout-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<IActionResult> LogoutAll()
    {
        var userId = this.GetCurrentUserId();
        if(userId == Guid.Empty)
            return BadRequest();

        var logoutCommand = new LogoutAllDevicesCommand(userId);
        
        var logoutResult = await _commandDispatcher.DispatchAsync(logoutCommand);
        if (!logoutResult.IsSuccess)
        {
            var problemDetails = new ValidationProblemDetails(logoutResult.Errors);
            return BadRequest(problemDetails);
        }
        
        HttpContext.Response.Cookies.Delete(_options.AuthCookieName);
        HttpContext.Response.Cookies.Delete(_options.RefreshCookieName);

        return Ok();
    }
}