using Clarity.Api.Contracts.AppTasks;
using FluentValidation;

namespace Clarity.Api.Validators.AppTasks;

public class AppTaskCreateRequestValidator : AbstractValidator<AppTaskCreateRequest>
{
    public AppTaskCreateRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required");
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required");
        RuleFor(x => x.Deadline)
            .NotEmpty()
            .WithMessage("Deadline is required");
        RuleFor(x => x.Deadline)
            .Must(d => d > (DateTime.Now).ToUniversalTime())
            .WithMessage("Deadline must be in the future");
    }
}