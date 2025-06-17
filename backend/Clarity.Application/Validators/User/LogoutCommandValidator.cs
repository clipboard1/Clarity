using Clarity.Application.Users.Logout;
using FluentValidation;

namespace Clarity.Application.Validators.User;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(c => c.Token)
            .NotEmpty()
            .WithMessage("Auth token cannot be empty");
    }
}