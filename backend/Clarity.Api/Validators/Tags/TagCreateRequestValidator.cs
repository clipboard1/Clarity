using Clarity.Api.Contracts.Tags;
using Clarity.Core.Models;
using FluentValidation;

namespace Clarity.Api.Validators.Tags;

public class TagCreateRequestValidator : AbstractValidator<TagCreateRequest>
{
    public TagCreateRequestValidator()
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
            
    }
}