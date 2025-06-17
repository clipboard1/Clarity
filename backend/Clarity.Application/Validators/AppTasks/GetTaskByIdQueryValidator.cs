using Clarity.Application.AppTasks.GetById;
using FluentValidation;

namespace Clarity.Application.Validators.AppTasks;

public class GetTaskByIdQueryValidator : AbstractValidator<GetTaskByIdQuery>
{
    public GetTaskByIdQueryValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("Task id cannot be empty");
    }
}