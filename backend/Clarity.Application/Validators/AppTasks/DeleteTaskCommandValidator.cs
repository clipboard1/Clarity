using Clarity.Application.AppTasks.Delete;
using FluentValidation;

namespace Clarity.Application.Validators.AppTasks;

public class DeleteTaskCommandValidator: AbstractValidator<DeleteTaskCommand>
{
    public DeleteTaskCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("Task id cannot be empty");
    }
}