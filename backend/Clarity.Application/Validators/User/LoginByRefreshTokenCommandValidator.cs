using Clarity.Application.Users.LoginByRefreshToken;
using FluentValidation;

namespace Clarity.Application.Validators.User;

public class LoginByRefreshTokenCommandValidator : 
    AbstractValidator<LoginByRefreshTokenCommand>
{
    public LoginByRefreshTokenCommandValidator()
    {
        RuleFor(c => c.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token cannot be empty");
    }
}