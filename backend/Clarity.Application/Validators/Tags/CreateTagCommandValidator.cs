using Clarity.Application.Tags.Create;
using Clarity.Core.Models;
using FluentValidation;

namespace Clarity.Application.Validators.Tags;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .Must(n => n.Length < Tag.MAX_NAME_LENGTH &&
                       n.Length >= Tag.MIN_NAME_LENGTH)
            .WithMessage($"Tag must be not empty and between " +
                         $"{Tag.MIN_NAME_LENGTH}" +
                         $"and {Tag.MAX_NAME_LENGTH} " +
                         $"characters");
        
        RuleFor(n => n.AppTaskId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("Task id cannot be empty");
        
        RuleFor(n => n.UserId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("User id cannot be empty");
    }
}