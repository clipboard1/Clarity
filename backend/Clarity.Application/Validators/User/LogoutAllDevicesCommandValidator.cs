using Clarity.Application.Users.LogoutAllDevices;
using FluentValidation;

namespace Clarity.Application.Validators.User;

public class LogoutAllDevicesCommandValidator : AbstractValidator<LogoutAllDevicesCommand>
{
    public LogoutAllDevicesCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("User id cannot be empty");
    }
}