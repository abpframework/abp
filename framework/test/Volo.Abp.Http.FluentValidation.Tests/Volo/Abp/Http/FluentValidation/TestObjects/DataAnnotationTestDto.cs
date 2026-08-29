using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Volo.Abp.Http.FluentValidation.TestObjects;

public class DataAnnotationTestDto
{
    [MinLength(2)]
    [MaxLength(50)]
    public string? MergedLengthValue { get; set; }

    [RegularExpression("^attribute$")]
    public string? AttributeRegexValue { get; set; }

    [Range(0, 100)]
    public int MergedRangeValue { get; set; }

    [Range(0, 100)]
    public int LooserFluentBoundValue { get; set; }

    [Range(10, 90)]
    public int SameBoundValue { get; set; }

    [Range(1, 100, MinimumIsExclusive = true, MaximumIsExclusive = true)]
    public int ExclusiveAttributeValue { get; set; }
}

public class DataAnnotationTestDtoValidator : AbstractValidator<DataAnnotationTestDto>
{
    public DataAnnotationTestDtoValidator()
    {
        RuleFor(x => x.MergedLengthValue).Length(5, 10);
        RuleFor(x => x.AttributeRegexValue).Matches("^fluent$");
        RuleFor(x => x.MergedRangeValue).GreaterThanOrEqualTo(10).LessThanOrEqualTo(90);
        RuleFor(x => x.LooserFluentBoundValue).GreaterThan(-5).LessThan(500);
        RuleFor(x => x.SameBoundValue).GreaterThan(10).LessThan(90);
    }
}
