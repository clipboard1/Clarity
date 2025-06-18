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
    }
}