using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Autofac;
using Volo.Abp.Http.FluentValidation.TestObjects;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.Http.FluentValidation;

public class FluentValidationApiDescription_Tests : AbpHttpFluentValidationTestBase<AbpHttpFluentValidationTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [Fact]
    public void Should_Register_The_Contributor()
    {
        ServiceProvider
            .GetServices<IPropertyApiDescriptionModelContributor>()
            .ShouldContain(x => x is FluentValidationPropertyApiDescriptionModelContributor);
    }

    [Fact]
    public async Task Should_Map_Required_Rules()
    {
        (await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.NotEmptyValue))).IsRequired.ShouldBeTrue();
        (await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.NotNullValue))).IsRequired.ShouldBeTrue();
        (await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.UnconstrainedValue))).IsRequired.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Map_Length_Rules()
    {
        var length = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.LengthValue));
        length.MinLength.ShouldBe(3);
        length.MaxLength.ShouldBe(10);

        var minimumLength = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.MinimumLengthValue));
        minimumLength.MinLength.ShouldBe(4);
        minimumLength.MaxLength.ShouldBeNull();

        var maximumLength = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.MaximumLengthValue));
        maximumLength.MinLength.ShouldBeNull();
        maximumLength.MaxLength.ShouldBe(12);
    }

    [Fact]
    public async Task Should_Not_Map_Zero_Length_Rules()
    {
        // A Func<T, int> bound is reported as a zero too, so a zero can not be published safely.
        (await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.EmptyOnlyValue))).MaxLength.ShouldBeNull();
        (await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.ZeroLengthValue))).MaxLength.ShouldBeNull();

        var dynamicLength = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.DynamicLengthValue));
        dynamicLength.MinLength.ShouldBeNull();
        dynamicLength.MaxLength.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Not_Map_A_Comparison_Bound_That_Is_Not_A_Number()
    {
        var property = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.DateComparisonValue));

        property.Minimum.ShouldBeNull();
        property.MinimumIsExclusive.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Map_A_Bound_Written_In_The_Exponent_Notation()
    {
        var property = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.SmallExponentValue));

        property.Minimum.ShouldBe("0.00000000000000000001");
    }

    [Fact]
    public async Task Should_Not_Map_A_Bound_Below_The_Decimal_Range()
    {
        // It would parse to a zero, which the server does not accept.
        var property = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.UnderflowExponentValue));

        property.Minimum.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Map_Regular_Expression_Rules()
    {
        var property = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.RegexValue));

        property.Regex.ShouldBe("^[a-z]+$");
    }

    [Fact]
    public async Task Should_Map_Inclusive_Comparison_Rules()
    {
        var minimum = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.InclusiveMinimumValue));
        minimum.Minimum.ShouldBe("5");
        minimum.MinimumIsExclusive.ShouldBe(false);

        var maximum = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.InclusiveMaximumValue));
        maximum.Maximum.ShouldBe("50");
        maximum.MaximumIsExclusive.ShouldBe(false);

        var between = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.InclusiveBetweenValue));
        between.Minimum.ShouldBe("1");
        between.Maximum.ShouldBe("10");
        between.MinimumIsExclusive.ShouldBe(false);
        between.MaximumIsExclusive.ShouldBe(false);
    }

    [Fact]
    public async Task Should_Map_Exclusive_Comparison_Rules()
    {
        var minimum = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.ExclusiveMinimumValue));
        minimum.Minimum.ShouldBe("5");
        minimum.MinimumIsExclusive.ShouldBe(true);

        var maximum = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.ExclusiveMaximumValue));
        maximum.Maximum.ShouldBe("50");
        maximum.MaximumIsExclusive.ShouldBe(true);

        var between = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.ExclusiveBetweenValue));
        between.Minimum.ShouldBe("1");
        between.Maximum.ShouldBe("10");
        between.MinimumIsExclusive.ShouldBe(true);
        between.MaximumIsExclusive.ShouldBe(true);
    }

    [Fact]
    public async Task Should_Not_Map_Comparison_Against_Another_Property()
    {
        var property = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.ComparedToOtherPropertyValue));

        property.Minimum.ShouldBeNull();
        property.Maximum.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Not_Map_RuleForEach_Rules_To_The_Collection_Property()
    {
        var property = await GetPropertyAsync<ConstraintTestDto>(nameof(ConstraintTestDto.CollectionValue));

        property.IsRequired.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Not_Map_Conditional_Rules()
    {
        (await GetPropertyAsync<ConditionalTestDto>(nameof(ConditionalTestDto.ChainedConditionValue))).IsRequired.ShouldBeFalse();
        (await GetPropertyAsync<ConditionalTestDto>(nameof(ConditionalTestDto.UnlessConditionValue))).IsRequired.ShouldBeFalse();
        (await GetPropertyAsync<ConditionalTestDto>(nameof(ConditionalTestDto.AsyncConditionValue))).IsRequired.ShouldBeFalse();
        (await GetPropertyAsync<ConditionalTestDto>(nameof(ConditionalTestDto.UnconditionalValue))).IsRequired.ShouldBeTrue();

        var blockCondition = await GetPropertyAsync<ConditionalTestDto>(nameof(ConditionalTestDto.BlockConditionValue));
        blockCondition.IsRequired.ShouldBeFalse();
        blockCondition.MaxLength.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Not_Map_Rules_Of_Named_Rule_Sets()
    {
        var namedRuleSet = await GetPropertyAsync<RuleSetTestDto>(nameof(RuleSetTestDto.NamedRuleSetValue));
        namedRuleSet.IsRequired.ShouldBeFalse();
        namedRuleSet.MinLength.ShouldBeNull();

        var defaultRuleSet = await GetPropertyAsync<RuleSetTestDto>(nameof(RuleSetTestDto.DefaultRuleSetValue));
        defaultRuleSet.IsRequired.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Keep_The_Stricter_Bound_When_Merging_With_Data_Annotations()
    {
        var length = await GetPropertyAsync<DataAnnotationTestDto>(nameof(DataAnnotationTestDto.MergedLengthValue));
        length.MinLength.ShouldBe(5);
        length.MaxLength.ShouldBe(10);

        var range = await GetPropertyAsync<DataAnnotationTestDto>(nameof(DataAnnotationTestDto.MergedRangeValue));
        range.Minimum.ShouldBe("10");
        range.Maximum.ShouldBe("90");
        range.MinimumIsExclusive.ShouldBe(false);
        range.MaximumIsExclusive.ShouldBe(false);
    }

    [Fact]
    public async Task Should_Read_Exclusive_Bounds_From_The_Range_Attribute()
    {
        var property = await GetPropertyAsync<DataAnnotationTestDto>(nameof(DataAnnotationTestDto.ExclusiveAttributeValue));

        property.Minimum.ShouldBe("1");
        property.Maximum.ShouldBe("100");
        property.MinimumIsExclusive.ShouldBe(true);
        property.MaximumIsExclusive.ShouldBe(true);
    }

    [Fact]
    public async Task Should_Keep_The_Exclusivity_Of_The_Winning_Bound()
    {
        // The attribute bounds are stricter, so the exclusive flags must not leak onto them.
        var looser = await GetPropertyAsync<DataAnnotationTestDto>(nameof(DataAnnotationTestDto.LooserFluentBoundValue));
        looser.Minimum.ShouldBe("0");
        looser.Maximum.ShouldBe("100");
        looser.MinimumIsExclusive.ShouldBe(false);
        looser.MaximumIsExclusive.ShouldBe(false);

        // On an equal bound the exclusive rule is the stricter one and wins.
        var same = await GetPropertyAsync<DataAnnotationTestDto>(nameof(DataAnnotationTestDto.SameBoundValue));
        same.Minimum.ShouldBe("10");
        same.Maximum.ShouldBe("90");
        same.MinimumIsExclusive.ShouldBe(true);
        same.MaximumIsExclusive.ShouldBe(true);
    }

    [Fact]
    public async Task Should_Keep_The_Attribute_Regular_Expression()
    {
        var property = await GetPropertyAsync<DataAnnotationTestDto>(nameof(DataAnnotationTestDto.AttributeRegexValue));

        property.Regex.ShouldBe("^attribute$");
    }

    [Fact]
    public async Task Should_Ignore_Types_Without_A_Validator()
    {
        var property = await GetPropertyAsync<UnvalidatedTestDto>(nameof(UnvalidatedTestDto.Value));

        property.IsRequired.ShouldBeFalse();
        property.MinLength.ShouldBeNull();
        property.MaxLength.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Ignore_Open_Generic_Types()
    {
        var typeModel = await CreateTypeModelAsync(typeof(GenericTestDto<>));
        var property = typeModel.Properties!.Single(x => x.Name == nameof(GenericTestDto<string>.Name));

        property.IsRequired.ShouldBeFalse();
        property.MinLength.ShouldBeNull();
        property.MaxLength.ShouldBeNull();
        property.Regex.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Merge_Range_Attribute_Bounds_Under_Any_Culture()
    {
        using (CultureHelper.Use(CultureInfo.GetCultureInfo("de-DE")))
        {
            var property = await GetPropertyAsync<CultureTestDto>(nameof(CultureTestDto.DecimalRangeValue));

            property.Minimum.ShouldBe("2");
            property.Maximum.ShouldBe("9.5");

            var typed = await GetPropertyAsync<CultureTestDto>(nameof(CultureTestDto.TypedDecimalRangeValue));
            typed.Minimum.ShouldBe("2");
            typed.Maximum.ShouldBe("9.5");
        }
    }

    [Fact]
    public async Task Should_Map_Rules_With_An_Overridden_Property_Name()
    {
        var property = await GetPropertyAsync<PropertyNameTestDto>(nameof(PropertyNameTestDto.RenamedValue));

        property.IsRequired.ShouldBeTrue();
        property.MaxLength.ShouldBe(20);
    }

    [Fact]
    public async Task Should_Not_Map_Rules_Of_A_Nested_Object_To_A_Property_Of_The_Same_Name()
    {
        var property = await GetPropertyAsync<PropertyNameTestDto>(nameof(PropertyNameTestDto.City));

        property.IsRequired.ShouldBeFalse();
        property.MaxLength.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Not_Map_Rules_Of_A_Nested_Object_To_The_Nested_Type()
    {
        // The model of the nested type is shared by every DTO that uses it, so a rule declared
        // by the validator of one of them can not be attributed to it.
        var property = await GetPropertyAsync<PropertyNameTestAddress>(nameof(PropertyNameTestAddress.City));

        property.IsRequired.ShouldBeFalse();
        property.MaxLength.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Not_Map_Rules_Of_A_Derived_Validator_To_A_Base_Property()
    {
        // Each type describes only the properties it declares, and the base type is described
        // with its own validator, which does not exist here.
        var inherited = await GetPropertyAsync<InheritanceTestBaseDto>(nameof(InheritanceTestBaseDto.InheritedValue));
        inherited.IsRequired.ShouldBeFalse();
        inherited.MaxLength.ShouldBeNull();

        var own = await GetPropertyAsync<InheritanceTestDto>(nameof(InheritanceTestDto.OwnValue));
        own.IsRequired.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Keep_A_Bound_That_Is_Not_A_Number()
    {
        var property = await GetPropertyAsync<CultureTestDto>(nameof(CultureTestDto.DateRangeValue));

        property.Minimum.ShouldBe("2020-01-01");
        property.Maximum.ShouldBe("2030-01-01");
    }
}
