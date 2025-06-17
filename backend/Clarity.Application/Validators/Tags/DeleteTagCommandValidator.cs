using Clarity.Application.Tags.Delete;
using FluentValidation;

namespace Clarity.Application.Validators.Tags;

public class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
{
    public DeleteTagCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .NotEqual(0)
            .WithMessage("Tag id cannot be empty");
    }
}