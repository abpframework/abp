using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Shouldly;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.Http.Modeling;

public class PropertyApiDescriptionModel_Tests
{
    [Fact]
    public void Create_Should_Write_Range_Bounds_With_The_Invariant_Culture()
    {
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("de-DE")))
        {
            var model = CreateModel(nameof(TestClass.DecimalRangeValue));

            model.Minimum.ShouldBe("1.5");
            model.Maximum.ShouldBe("9.5");
        }
    }

    [Fact]
    public void Create_Should_Write_Typed_Range_Bounds_With_The_Invariant_Culture()
    {
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("de-DE")))
        {
            var model = CreateModel(nameof(TestClass.TypedDecimalRangeValue));

            model.Minimum.ShouldBe("1.5");
            model.Maximum.ShouldBe("9.5");
        }
    }

    [Fact]
    public void Create_Should_Convert_A_Typed_Range_Bound_With_Its_Operand_Type()
    {
        // A magnitude outside the decimal range is still a valid double bound.
        var model = CreateModel(nameof(TestClass.ExponentRangeValue));

        model.Minimum.ShouldBe("1E-30");
        model.Maximum.ShouldBe("1E+30");
    }

    [Fact]
    public void Create_Should_Convert_A_Typed_Double_Range_Bound_Written_In_Another_Culture()
    {
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("de-DE")))
        {
            var model = CreateModel(nameof(TestClass.CultureExponentRangeValue));

            model.Minimum.ShouldBe("1.5E+30");
            model.Maximum.ShouldBe("9.5E+30");
        }
    }

    [Fact]
    public void Create_Should_Keep_A_Typed_Range_Bound_That_Is_Not_A_Number()
    {
        var model = CreateModel(nameof(TestClass.DateRangeValue));

        model.Minimum.ShouldBe("2020-01-01");
        model.Maximum.ShouldBe("2030-01-01");
    }

    [Fact]
    public void Create_Should_Read_The_Exclusive_Bounds_Of_The_Range_Attribute()
    {
        var model = CreateModel(nameof(TestClass.ExclusiveRangeValue));

        model.Minimum.ShouldBe("1");
        model.Maximum.ShouldBe("100");
        model.MinimumIsExclusive.ShouldBe(true);
        model.MaximumIsExclusive.ShouldBe(true);
    }

    [Fact]
    public void Create_Should_Report_An_Inclusive_Range_Attribute_As_Inclusive()
    {
        var model = CreateModel(nameof(TestClass.DecimalRangeValue));

        model.MinimumIsExclusive.ShouldBe(false);
        model.MaximumIsExclusive.ShouldBe(false);
    }

    [Fact]
    public void Create_Should_Leave_The_Exclusive_Bounds_Null_Without_A_Range_Attribute()
    {
        var model = CreateModel(nameof(TestClass.UnconstrainedValue));

        model.Minimum.ShouldBeNull();
        model.Maximum.ShouldBeNull();
        model.MinimumIsExclusive.ShouldBeNull();
        model.MaximumIsExclusive.ShouldBeNull();
    }

    private static PropertyApiDescriptionModel CreateModel(string propertyName)
    {
        return PropertyApiDescriptionModel.Create(typeof(TestClass).GetProperty(propertyName)!);
    }

    public class TestClass
    {
        [Range(1.5, 9.5)]
        public double DecimalRangeValue { get; set; }

        [Range(typeof(decimal), "1,5", "9,5")]
        public decimal TypedDecimalRangeValue { get; set; }

        [Range(typeof(double), "1e-30", "1e30", ParseLimitsInInvariantCulture = true)]
        public double ExponentRangeValue { get; set; }

        [Range(typeof(double), "1,5E+30", "9,5E+30")]
        public double CultureExponentRangeValue { get; set; }

        [Range(typeof(DateTime), "2020-01-01", "2030-01-01")]
        public DateTime DateRangeValue { get; set; }

        [Range(1, 100, MinimumIsExclusive = true, MaximumIsExclusive = true)]
        public int ExclusiveRangeValue { get; set; }

        public string? UnconstrainedValue { get; set; }
    }
}
