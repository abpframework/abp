using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Volo.Abp.Http.FluentValidation.TestObjects;

public class CultureTestDto
{
    [Range(1.5, 9.5)]
    public double DecimalRangeValue { get; set; }

    [Range(typeof(decimal), "1,5", "9,5")]
    public decimal TypedDecimalRangeValue { get; set; }

    [Range(typeof(double), "1e-30", "1e30", ParseLimitsInInvariantCulture = true)]
    public double ExponentRangeValue { get; set; }

    [Range(typeof(DateTime), "2020-01-01", "2030-01-01")]
    public DateTime DateRangeValue { get; set; }
}

public class CultureTestDtoValidator : AbstractValidator<CultureTestDto>
{
    public CultureTestDtoValidator()
    {
        RuleFor(x => x.DecimalRangeValue).GreaterThanOrEqualTo(2.0);
        RuleFor(x => x.TypedDecimalRangeValue).GreaterThanOrEqualTo(2m);
        RuleFor(x => x.ExponentRangeValue).GreaterThanOrEqualTo(2d);
    }
}
