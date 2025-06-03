using System.ComponentModel.DataAnnotations;

namespace Clarity.Api.Contracts.Users;

public record RegisterUserRequest(
    [Required] string Username,
    [Required] string Email,
    [Required] string Password);