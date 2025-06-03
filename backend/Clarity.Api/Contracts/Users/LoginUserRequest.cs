using System.ComponentModel.DataAnnotations;

namespace Clarity.Api.Contracts.Users;

public record LoginUserRequest(
    [Required] string Email,
    [Required] string Password);