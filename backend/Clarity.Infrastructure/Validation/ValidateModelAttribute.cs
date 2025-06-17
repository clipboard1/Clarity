using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Clarity.Infrastructure.Validation;

public class ValidateModelAttribute<T> : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments)
        {
            if (argument.Value is T model)
            {
                var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
                if (validator != null)
                {
                    var validationResult = await validator.ValidateAsync(model);
                    if (!validationResult.IsValid)
                    {
                        context.Result = new BadRequestObjectResult(new ValidationProblemDetails
                        {
                            Errors = validationResult.Errors
                                .GroupBy(e => e.PropertyName)
                                .ToDictionary(
                                    el => el.Key,
                                    el => 
                                        el.Select(p => p.ErrorMessage)
                                            .ToArray())
                        });
                        return;
                    }
                }
                break;
            } 
        }
        await next();
    }
}