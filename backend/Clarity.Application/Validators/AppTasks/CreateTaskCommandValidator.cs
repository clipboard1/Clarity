using Clarity.Application.AppTasks.Create;
using Clarity.Core.Models;
using FluentValidation;

namespace Clarity.Application.Validators.AppTasks;

public class CreateTaskCommandValidator  : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("User id cannot be empty");
        RuleFor(c => c.Title)
            .NotEmpty()
            .Must(t => t.Length < AppTask.MAX_TITLE_LENGTH &&
                       t.Length > AppTask.MIN_TITLE_LENGTH)
            .WithMessage($"Title must be not empty and between " +
                         $"{AppTask.MIN_TITLE_LENGTH}" +
                         $"and {AppTask.MAX_TITLE_LENGTH} " +
                         $"characters");
        RuleFor(c => c.Description)
            .NotEmpty()
            .Must(d => d.Length < AppTask.MAX_DESCRIPTION_LENGTH)
            .WithMessage($"Description must be not empty and smaler than " +
                         $"{AppTask.MAX_DESCRIPTION_LENGTH}" +
                         $"characters");
        RuleFor(x => x.Status)
            .Must(Enum.IsDefined)
            .WithMessage("Deadline must be in the future");

    }
}