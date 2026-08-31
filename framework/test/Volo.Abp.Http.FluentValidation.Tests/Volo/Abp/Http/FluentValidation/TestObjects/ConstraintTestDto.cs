using System;
using System.Collections.Generic;
using FluentValidation;

namespace Volo.Abp.Http.FluentValidation.TestObjects;

public class ConstraintTestDto
{
    public string? NotEmptyValue { get; set; }

    public string? NotNullValue { get; set; }

    public string? LengthValue { get; set; }

    public string? MinimumLengthValue { get; set; }

    public string? MaximumLengthValue { get; set; }

    public string? RegexValue { get; set; }

    public int InclusiveMinimumValue { get; set; }

    public int InclusiveMaximumValue { get; set; }

    public int ExclusiveMinimumValue { get; set; }

    public int ExclusiveMaximumValue { get; set; }

    public int InclusiveBetweenValue { get; set; }

    public int ExclusiveBetweenValue { get; set; }

    public int ComparedToOtherPropertyValue { get; set; }

    public List<string> CollectionValue { get; set; } = new List<string>();

    public string? EmptyOnlyValue { get; set; }

    public string? ZeroLengthValue { get; set; }

    public string? DynamicLengthValue { get; set; }

    public DateTime DateComparisonValue { get; set; }

    public double SmallExponentValue { get; set; }

    public double UnderflowExponentValue { get; set; }

    public string? StringComparisonValue { get; set; }

    public string? UnconstrainedValue { get; set; }
}

public class ConstraintTestDtoValidator : AbstractValidator<ConstraintTestDto>
{
    public ConstraintTestDtoValidator()
    {
        RuleFor(x => x.NotEmptyValue).NotEmpty();
        RuleFor(x => x.NotNullValue).NotNull();
        RuleFor(x => x.LengthValue).Length(3, 10);
        RuleFor(x => x.MinimumLengthValue).MinimumLength(4);
        RuleFor(x => x.MaximumLengthValue).MaximumLength(12);
        RuleFor(x => x.RegexValue).Matches("^[a-z]+$");
        RuleFor(x => x.InclusiveMinimumValue).GreaterThanOrEqualTo(5);
        RuleFor(x => x.InclusiveMaximumValue).LessThanOrEqualTo(50);
        RuleFor(x => x.ExclusiveMinimumValue).GreaterThan(5);
        RuleFor(x => x.ExclusiveMaximumValue).LessThan(50);
        RuleFor(x => x.InclusiveBetweenValue).InclusiveBetween(1, 10);
        RuleFor(x => x.ExclusiveBetweenValue).ExclusiveBetween(1, 10);
        RuleFor(x => x.ComparedToOtherPropertyValue).GreaterThanOrEqualTo(x => x.InclusiveMinimumValue);
        RuleForEach(x => x.CollectionValue).NotEmpty();
        RuleFor(x => x.EmptyOnlyValue).MaximumLength(0);
        RuleFor(x => x.ZeroLengthValue).Length(0, 0);
        RuleFor(x => x.DateComparisonValue).GreaterThanOrEqualTo(new DateTime(2020, 1, 1));
        RuleFor(x => x.DynamicLengthValue).Length(_ => 2, _ => 8);
        RuleFor(x => x.SmallExponentValue).GreaterThanOrEqualTo(1e-20);
        RuleFor(x => x.UnderflowExponentValue).GreaterThanOrEqualTo(1e-30);
        RuleFor(x => x.StringComparisonValue).GreaterThan("10");
    }
}
