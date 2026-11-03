using System.Threading.Tasks;
using FluentValidation;

namespace Volo.Abp.Http.FluentValidation.TestObjects;

public class ConditionalTestDto
{
    public bool Enabled { get; set; }

    public string? ChainedConditionValue { get; set; }

    public string? BlockConditionValue { get; set; }

    public string? UnlessConditionValue { get; set; }

    public string? AsyncConditionValue { get; set; }

    public string? UnconditionalValue { get; set; }
}

public class ConditionalTestDtoValidator : AbstractValidator<ConditionalTestDto>
{
    public ConditionalTestDtoValidator()
    {
        RuleFor(x => x.ChainedConditionValue).NotEmpty().When(x => x.Enabled);

        When(x => x.Enabled, () =>
        {
            RuleFor(x => x.BlockConditionValue).NotEmpty().MaximumLength(32);
        });

        Unless(x => x.Enabled, () =>
        {
            RuleFor(x => x.UnlessConditionValue).NotEmpty();
        });

        RuleFor(x => x.AsyncConditionValue).NotEmpty().WhenAsync((x, _) => Task.FromResult(x.Enabled));

        RuleFor(x => x.UnconditionalValue).NotEmpty();
    }
}
