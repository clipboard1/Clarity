using Clarity.Application.AppTasks.GetAll;
using FluentValidation;

namespace Clarity.Application.Validators.AppTasks;

public class GetAllTasksQueryValidator : AbstractValidator<GetAllTasksQuery>
{
    public GetAllTasksQueryValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("User id cannot be empty");
    }
}