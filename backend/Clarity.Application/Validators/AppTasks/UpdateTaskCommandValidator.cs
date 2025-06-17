using Clarity.Application.AppTasks.Update;
using FluentValidation;

namespace Clarity.Application.Validators.AppTasks;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("Task id cannot be empty");
        RuleFor(c => c.UserId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("User id cannot be empty");
        RuleFor(c => c.Update)
            .NotEmpty()
            .WithMessage("Update data cannot be empty");
    }
}