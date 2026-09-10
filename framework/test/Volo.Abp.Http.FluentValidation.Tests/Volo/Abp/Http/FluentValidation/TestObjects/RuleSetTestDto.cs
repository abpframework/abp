using FluentValidation;

namespace Volo.Abp.Http.FluentValidation.TestObjects;

public class RuleSetTestDto
{
    public string? NamedRuleSetValue { get; set; }

    public string? DefaultRuleSetValue { get; set; }
}

public class RuleSetTestDtoValidator : AbstractValidator<RuleSetTestDto>
{
    public RuleSetTestDtoValidator()
    {
        RuleSet("Create", () =>
        {
            RuleFor(x => x.NamedRuleSetValue).NotEmpty().MinimumLength(4);
        });

        RuleSet("default", () =>
        {
            RuleFor(x => x.DefaultRuleSetValue).NotEmpty();
        });
    }
}
