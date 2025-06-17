using Clarity.Api.Contracts.Users;
using FluentValidation;

namespace Clarity.Api.Validators.Users;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
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