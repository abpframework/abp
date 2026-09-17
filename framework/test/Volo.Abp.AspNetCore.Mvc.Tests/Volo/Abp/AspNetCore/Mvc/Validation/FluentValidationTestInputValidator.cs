using FluentValidation;
using Volo.Abp.TestApp.Application;

namespace Volo.Abp.AspNetCore.Mvc.Validation;

public class FluentValidationTestInputValidator : AbstractValidator<FluentValidationTestInput>
{
    public FluentValidationTestInputValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(10);
    }
}
