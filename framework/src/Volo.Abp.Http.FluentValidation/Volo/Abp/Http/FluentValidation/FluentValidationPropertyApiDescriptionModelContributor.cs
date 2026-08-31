using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.FluentValidation;

[ExposeServices(typeof(IPropertyApiDescriptionModelContributor))]
public class FluentValidationPropertyApiDescriptionModelContributor : IPropertyApiDescriptionModelContributor, ITransientDependency
{
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
            ApplyValidator(context.Model, validator);
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

    protected virtual void ApplyValidator(PropertyApiDescriptionModel model, IPropertyValidator validator)
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
                ApplyBetween(model, betweenValidator);
                break;
            case IComparisonValidator comparisonValidator:
                ApplyComparison(model, comparisonValidator);
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

    protected virtual void ApplyComparison(PropertyApiDescriptionModel model, IComparisonValidator validator)
    {
        switch (validator.Comparison)
        {
            case Comparison.GreaterThan:
                ApplyMinimum(model, validator.ValueToCompare, isExclusive: true);
                break;
            case Comparison.GreaterThanOrEqual:
                ApplyMinimum(model, validator.ValueToCompare, isExclusive: false);
                break;
            case Comparison.LessThan:
                ApplyMaximum(model, validator.ValueToCompare, isExclusive: true);
                break;
            case Comparison.LessThanOrEqual:
                ApplyMaximum(model, validator.ValueToCompare, isExclusive: false);
                break;
        }
    }

    protected virtual void ApplyBetween(PropertyApiDescriptionModel model, IBetweenValidator validator)
    {
        var isExclusive = validator is not IInclusiveBetweenValidator;

        ApplyMinimum(model, validator.From, isExclusive);
        ApplyMaximum(model, validator.To, isExclusive);
    }

    protected virtual void ApplyMinimum(PropertyApiDescriptionModel model, object? value, bool isExclusive)
    {
        // Minimum and Maximum are ordered bounds, so a value that is not a number, such as a
        // DateTime, has nothing meaningful to publish there.
        if (!TryGetNumber(value, out var minimum))
        {
            return;
        }

        if (model.Minimum != null)
        {
            if (!TryParseNumber(model.Minimum, out var existingMinimum))
            {
                return;
            }

            // The flag follows the winning bound instead of being combined, otherwise ">= 10" merged with "> 5" would become "> 10".
            var existingIsExclusive = model.MinimumIsExclusive == true;
            if (existingMinimum > minimum || (existingMinimum == minimum && existingIsExclusive))
            {
                minimum = existingMinimum;
                isExclusive = existingIsExclusive;
            }
        }

        model.Minimum = minimum.ToString(CultureInfo.InvariantCulture);
        model.MinimumIsExclusive = isExclusive;
    }

    protected virtual void ApplyMaximum(PropertyApiDescriptionModel model, object? value, bool isExclusive)
    {
        if (!TryGetNumber(value, out var maximum))
        {
            return;
        }

        if (model.Maximum != null)
        {
            if (!TryParseNumber(model.Maximum, out var existingMaximum))
            {
                return;
            }

            var existingIsExclusive = model.MaximumIsExclusive == true;
            if (existingMaximum < maximum || (existingMaximum == maximum && existingIsExclusive))
            {
                maximum = existingMaximum;
                isExclusive = existingIsExclusive;
            }
        }

        model.Maximum = maximum.ToString(CultureInfo.InvariantCulture);
        model.MaximumIsExclusive = isExclusive;
    }

    protected virtual bool TryGetNumber(object? value, out decimal number)
    {
        // A comparison against another property has no value to read.
        var bound = value != null ? Convert.ToString(value, CultureInfo.InvariantCulture) : null;
        return TryParseNumber(bound, out number);
    }

    protected virtual bool TryParseNumber(string? value, out decimal number)
    {
        // Float allows the exponent notation but not the group separators, which a Range bound
        // rendered by a decimal-comma culture would otherwise smuggle in as "1,5" meaning 15.
        if (!decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out number))
        {
            return false;
        }

        // A magnitude below the decimal range parses to zero, which would publish a bound the
        // server does not enforce.
        return number != decimal.Zero || !value!.Any(c => c is > '0' and <= '9');
    }
}
