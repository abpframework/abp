using FluentValidation;

namespace Volo.Abp.Http.FluentValidation.TestObjects;

public class InheritanceTestBaseDto
{
    public string? InheritedValue { get; set; }
}

public class InheritanceTestDto : InheritanceTestBaseDto
{
    public string? OwnValue { get; set; }
}

public class InheritanceTestDtoValidator : AbstractValidator<InheritanceTestDto>
{
    public InheritanceTestDtoValidator()
    {
        RuleFor(x => x.InheritedValue).NotEmpty().MaximumLength(15);
        RuleFor(x => x.OwnValue).NotEmpty();
    }
}
