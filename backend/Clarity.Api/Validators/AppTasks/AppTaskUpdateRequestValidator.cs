using Clarity.Api.Contracts.AppTasks;
using FluentValidation;

namespace Clarity.Api.Validators.AppTasks;

public class AppTaskUpdateRequestValidator : AbstractValidator<AppTaskUpdateRequest>
{
    public AppTaskUpdateRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .When(x => x.Title != null)
            .WithMessage("Title cannot be empty if provided");
        RuleFor(x => x.Description)
            .NotEmpty()
            .When(x => x.Description != null)
            .WithMessage("Description cannot be empty if provided");
        RuleFor(x => x.Deadline)
            .NotEmpty()
            .When(x => x.Deadline.HasValue)
            .WithMessage("Deadline cannot be empty if provided");
        RuleFor(x => x.Deadline)
            .Must(d => d > (DateTime.Now).ToUniversalTime())
            .When(x => x.Deadline.HasValue)
            .WithMessage("Deadline cannot be in past if provided");
        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue)
            .WithMessage("Status must be valid value if provided");
    }
}