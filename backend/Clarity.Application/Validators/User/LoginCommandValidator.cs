using Clarity.Application.Users.Login;
using FluentValidation;

namespace Clarity.Application.Validators.User;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty");
    }
}