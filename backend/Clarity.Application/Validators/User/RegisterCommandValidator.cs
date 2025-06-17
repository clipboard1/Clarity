using Clarity.Application.Users.Register;
using FluentValidation;

namespace Clarity.Application.Validators.User;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username cannot be empty");
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty");
    }
}