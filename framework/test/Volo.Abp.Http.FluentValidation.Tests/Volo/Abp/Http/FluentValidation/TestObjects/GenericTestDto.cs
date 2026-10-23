using FluentValidation;

namespace Volo.Abp.Http.FluentValidation.TestObjects;

public class GenericTestDto<T>
{
    public T? Value { get; set; }

    public string? Name { get; set; }
}

public class GenericTestDtoValidator : AbstractValidator<GenericTestDto<string>>
{
    public GenericTestDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(3, 9).Matches("^[a-z]+$");
    }
}
