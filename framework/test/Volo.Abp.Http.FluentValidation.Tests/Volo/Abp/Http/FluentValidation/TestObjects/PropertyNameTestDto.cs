using FluentValidation;

namespace Volo.Abp.Http.FluentValidation.TestObjects;

public class PropertyNameTestDto
{
    public string? RenamedValue { get; set; }

    public string? City { get; set; }

    public PropertyNameTestAddress Address { get; set; } = new PropertyNameTestAddress();
}

public class PropertyNameTestAddress
{
    public string? City { get; set; }
}

public class PropertyNameTestDtoValidator : AbstractValidator<PropertyNameTestDto>
{
    public PropertyNameTestDtoValidator()
    {
        RuleFor(x => x.RenamedValue).NotEmpty().MaximumLength(20).OverridePropertyName("displayed_name");
        RuleFor(x => x.Address.City).NotEmpty().MaximumLength(30);
    }
}
