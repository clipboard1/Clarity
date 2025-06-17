using Clarity.Application.Tags.GetAllByTask;
using FluentValidation;

namespace Clarity.Application.Validators.Tags;

public class GetAllTagsByTaskQueryValidator : 
    AbstractValidator<GetAllTagsByTaskQuery>
{
    public GetAllTagsByTaskQueryValidator()
    {
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