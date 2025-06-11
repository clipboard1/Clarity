using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Clarity.Api.Extensions;

public static class ControllerExtension
{
    public static Guid GetCurrentUserId(this ControllerBase controller) =>
        Guid.Parse(controller.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? throw new UnauthorizedAccessException("User ID not found."));
}