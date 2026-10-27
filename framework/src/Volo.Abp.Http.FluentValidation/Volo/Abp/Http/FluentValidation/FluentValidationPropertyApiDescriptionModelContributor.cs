using System;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Reflection;

namespace Volo.Abp.Http.FluentValidation;

[ExposeServices(typeof(IPropertyApiDescriptionModelContributor))]
public class FluentValidationPropertyApiDescriptionModelContributor : IPropertyApiDescriptionModelContributor, ITransientDependency
{
    private static readonly FrozenSet<Type> NumericTypes = new HashSet<Type>
    {
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
        typeof(float),
        typeof(double),
        typeof(decimal),
        typeof(IntPtr),
        typeof(UIntPtr)
    }.ToFrozenSet();

    protected IServiceProvider ServiceProvider { get; }

    protected ConcurrentDictionary<Type, ILookup<string, IPropertyValidator>?> RuleCache { get; }

    public FluentValidationPropertyApiDescriptionModelContributor(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        RuleCache = new ConcurrentDictionary<Type, ILookup<string, IPropertyValidator>?>();
    }

    public virtual Task ContributeAsync(PropertyApiDescriptionModelContributionContext context)
    {
        Check.NotNull(context, nameof(context));

        var rules = RuleCache.GetOrAdd(context.DeclaringType, GetUnconditionalRules);
        if (rules == null)
        {
            return Task.CompletedTask;
        }

        foreach (var validator in rules[context.Model.Name])
        {
            ApplyValidator(context.Model, context.PropertyInfo, validator);
        }

        return Task.CompletedTask;
    }

    protected virtual ILookup<string, IPropertyValidator>? GetUnconditionalRules(Type declaringType)
    {
        if (declaringType.ContainsGenericParameters)
        {
            return null;
        }

        var validator = ServiceProvider.GetService(typeof(IValidator<>).MakeGenericType(declaringType)) as IValidator;
        if (validator == null)
        {
            return null;
        }

        return validator
            .CreateDescriptor()
            .Rules
            .Where(rule => IsUnconditional(rule) && DescribesPropertyOf(rule, declaringType))
            .SelectMany(rule => rule
                .Components
                .Where(component => !component.HasCondition && !component.HasAsyncCondition)
                .Select(component => new KeyValuePair<string, IPropertyValidator>(rule.Member.Name, component.Validator)))
            .ToLookup(x => x.Key, x => x.Value, StringComparer.Ordinal);
    }

    protected virtual bool DescribesPropertyOf(IValidationRule rule, Type declaringType)
    {
        // The member is matched instead of the rule's property name, which OverridePropertyName
        // and a custom PropertyNameResolver can change. A rule on a nested object declares its
        // member on that object, so it must not be attributed to a property of this type.
        return rule.Member != null && rule.Member.DeclaringType!.IsAssignableFrom(declaringType);
    }

    protected virtual bool IsUnconditional(IValidationRule rule)
    {

        // RuleForEach constrains the items, not the collection property itself.
        if (IsCollectionRule(rule))
        {
            return false;
        }

        // A When(...)/Unless(...) block sets the condition on the rule, the chained form sets it on the components.
        if (rule.HasCondition || rule.HasAsyncCondition)
        {
            return false;
        }

        // Mirrors DefaultValidatorSelector: ABP never runs a rule that only belongs to a named rule set.
        return rule.RuleSets.IsNullOrEmpty() ||
               rule.RuleSets!.Contains(RulesetValidatorSelector.DefaultRuleSetName, StringComparer.OrdinalIgnoreCase);
    }

    protected virtual bool IsCollectionRule(IValidationRule rule)
    {
        return rule
            .GetType()
            .GetInterfaces()
            .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollectionRule<,>));
    }

    protected virtual void ApplyValidator(PropertyApiDescriptionModel model, PropertyInfo propertyInfo, IPropertyValidator validator)
    {
        switch (validator)
        {
            case INotNullValidator:
            case INotEmptyValidator:
                model.IsRequired = true;
                break;
            case ILengthValidator lengthValidator:
                ApplyLength(model, lengthValidator);
                break;
            case IRegularExpressionValidator regularExpressionValidator:
                ApplyRegularExpression(model, regularExpressionValidator);
                break;
            case IBetweenValidator betweenValidator:
                ApplyBetween(model, propertyInfo, betweenValidator);
                break;
            case IComparisonValidator comparisonValidator:
                ApplyComparison(model, propertyInfo, comparisonValidator);
                break;
        }
    }

    protected virtual void ApplyLength(PropertyApiDescriptionModel model, ILengthValidator validator)
    {
        // Every length validator has a Func<T,int> form that leaves the bound at zero on the
        // descriptor, so a zero is not distinguishable from a literal one and is left out.
        if (validator.Min > 0)
        {
            model.MinLength = model.MinLength.HasValue
                ? Math.Max(model.MinLength.Value, validator.Min)
                : validator.Min;
        }

        if (validator.Max > 0)
        {
            model.MaxLength = model.MaxLength.HasValue
                ? Math.Min(model.MaxLength.Value, validator.Max)
                : validator.Max;
        }
    }

    protected virtual void ApplyRegularExpression(PropertyApiDescriptionModel model, IRegularExpressionValidator validator)
    {
        // A single Regex field cannot express "must match all of them", so the first pattern wins.
        if (!model.Regex.IsNullOrWhiteSpace() || validator.Expression.IsNullOrWhiteSpace())
        {
            return;
        }

        model.Regex = validator.Expression;
    }

    protected virtual void ApplyComparison(PropertyApiDescriptionModel model, PropertyInfo propertyInfo, IComparisonValidator validator)
    {
        switch (validator.Comparison)
        {
            case Comparison.GreaterThan:
                ApplyMinimum(model, propertyInfo, validator.ValueToCompare, isExclusive: true);
                break;
            case Comparison.GreaterThanOrEqual:
                ApplyMinimum(model, propertyInfo, validator.ValueToCompare, isExclusive: false);
                break;
            case Comparison.LessThan:
                ApplyMaximum(model, propertyInfo, validator.ValueToCompare, isExclusive: true);
                break;
            case Comparison.LessThanOrEqual:
                ApplyMaximum(model, propertyInfo, validator.ValueToCompare, isExclusive: false);
                break;
        }
    }

    protected virtual void ApplyBetween(PropertyApiDescriptionModel model, PropertyInfo propertyInfo, IBetweenValidator validator)
    {
        var from = GetNumericBound(propertyInfo, validator.From);
        var to = GetNumericBound(propertyInfo, validator.To);

        // A between rule can carry its own comparer, which the descriptor does not expose. An
        // interval that reads as empty in the natural order is what one looks like from here,
        // and publishing its bounds would say the opposite of what the rule accepts.
        if (from == null || to == null || !TryCompareBounds(from, to, out var comparison) || comparison > 0)
        {
            return;
        }

        var isExclusive = validator is not IInclusiveBetweenValidator;

        ApplyMinimum(model, propertyInfo, validator.From, isExclusive);
        ApplyMaximum(model, propertyInfo, validator.To, isExclusive);
    }

    protected virtual void ApplyMinimum(PropertyApiDescriptionModel model, PropertyInfo propertyInfo, object? value, bool isExclusive)
    {
        var bound = GetNumericBound(propertyInfo, value);
        if (bound == null)
        {
            return;
        }

        if (model.Minimum != null)
        {
            if (!TryCompareBounds(model.Minimum, bound, out var comparison))
            {
                return;
            }

            // The higher bound wins, and an exclusive one is the stricter when both sit on the
            // same value. The winning bound is published the way it was written, so no value is
            // lost on the way through a number type that can not hold it.
            if (comparison > 0 || (comparison == 0 && model.MinimumIsExclusive == true))
            {
                return;
            }
        }

        model.Minimum = bound;
        model.MinimumIsExclusive = isExclusive;
    }

    protected virtual void ApplyMaximum(PropertyApiDescriptionModel model, PropertyInfo propertyInfo, object? value, bool isExclusive)
    {
        var bound = GetNumericBound(propertyInfo, value);
        if (bound == null)
        {
            return;
        }

        if (model.Maximum != null)
        {
            if (!TryCompareBounds(model.Maximum, bound, out var comparison))
            {
                return;
            }

            if (comparison < 0 || (comparison == 0 && model.MaximumIsExclusive == true))
            {
                return;
            }
        }

        model.Maximum = bound;
        model.MaximumIsExclusive = isExclusive;
    }

    protected virtual string? GetNumericBound(PropertyInfo propertyInfo, object? value)
    {
        // Minimum and Maximum are numeric bounds. A comparison on another type, the ordinal
        // comparison of two strings for example, means something else and can not go there.
        // A comparison against another property has no value to publish either.
        if (value == null || !NumericTypes.Contains(TypeHelper.StripNullable(propertyInfo.PropertyType)))
        {
            return null;
        }

        var bound = Convert.ToString(value, CultureInfo.InvariantCulture);
        return bound.IsNullOrWhiteSpace() || !double.TryParse(bound, NumberStyles.Float, CultureInfo.InvariantCulture, out _)
            ? null
            : bound;
    }

    protected virtual bool TryCompareBounds(string left, string right, out int comparison)
    {
        // Decimal is exact for every integral type and for decimal itself, which double is not
        // above its 53 bits of mantissa. Double only comes in for the magnitudes decimal can
        // not hold, where its precision is the best there is anyway.
        if (TryParseExactly(left, out var leftValue) && TryParseExactly(right, out var rightValue))
        {
            comparison = leftValue.CompareTo(rightValue);
            return true;
        }

        if (double.TryParse(left, NumberStyles.Float, CultureInfo.InvariantCulture, out var leftDouble) &&
            double.TryParse(right, NumberStyles.Float, CultureInfo.InvariantCulture, out var rightDouble))
        {
            comparison = leftDouble.CompareTo(rightDouble);
            return true;
        }

        comparison = 0;
        return false;
    }

    protected virtual bool TryParseExactly(string value, out decimal number)
    {
        if (!decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out number))
        {
            return false;
        }

        // A magnitude below the decimal range collapses to a zero, which would compare wrong.
        return number != decimal.Zero || !value.Any(c => c is > '0' and <= '9');
    }
}
