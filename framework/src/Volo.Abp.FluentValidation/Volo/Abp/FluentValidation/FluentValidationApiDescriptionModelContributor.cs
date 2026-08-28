using FluentValidation;
using FluentValidation.Validators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.FluentValidation;

public class FluentValidationApiDescriptionModelContributor : IPropertyApiDescriptionModelContributor
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = default!;
    public virtual void Contribute(
        PropertyApiDescriptionModel model,
        PropertyInfo propertyInfo)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(propertyInfo);

        var declaringType = propertyInfo.DeclaringType;

        if (declaringType == null)
        {
            return;
        }

        // Guard against open generic types (e.g. a property inherited from
        // a generic base DTO like ExtensibleFullAuditedEntityDto<TPrimaryKey>
        // where TPrimaryKey hasn't been substituted with a concrete type).
        // There is no real IValidator<> for an open generic, so DI can never
        // have it registered — attempting the lookup would either throw or
        // be meaningless.
        if (declaringType.ContainsGenericParameters)
        {
            return;
        }

        var validatorType =
            typeof(IValidator<>).MakeGenericType(declaringType);

        var validators =
            LazyServiceProvider
            .GetServices(validatorType)
            .ToArray();

        if (validators.Length == 0)
        {
            return;
        }

        foreach (var validator in validators)
        {

            if (validator is IValidator typedValidator)
            {

                ApplyValidatorRules(
                    model,
                    propertyInfo,
                    typedValidator);
            }
        }
    }

    protected virtual void ApplyValidatorRules(
        PropertyApiDescriptionModel model,
        PropertyInfo propertyInfo,
        IValidator validator)
    {
        var descriptor = validator.CreateDescriptor();

        var rule = descriptor
                .GetMembersWithValidators()
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Key,
                        propertyInfo.Name,
                        StringComparison.Ordinal));

        if (rule == null)
        {
            return;
        }

        foreach (var (Validator, Options) in rule)
        {
            /*
             * Only statically-evaluable validators are reflected here.
             *
             * FluentValidation conditions such as:
             *
             *     When(...)
             *     Unless(...)
             *
             * are intentionally ignored. They depend on the runtime state
             * of the object being validated (other property values, external
             * context, etc.), so there is no single correct answer to
             * "is this required?" at the type/schema level — the same
             * property could be required in one instance and optional in
             * another. Baking a conditional rule into a static schema would
             * misrepresent it either way, so we skip it rather than guess.
             *
             * If the validator is a custom/unsupported type, it simply
             * won't match one of the cases below and is ignored.
             */
            if (Options.HasCondition || Options.HasAsyncCondition)
                continue;
            ApplyValidator(
                model,
                Validator);
        }
    }

    protected virtual void ApplyValidator(
        PropertyApiDescriptionModel model,
        IPropertyValidator validator)
    {
        switch (validator)
        {
            // ============================================================
            // Required
            //
            // NotNull()  -> value must not be null (empty string/whitespace
            //               still pass)
            // NotEmpty() -> value must not be null AND not the "empty"
            //               value for its type (empty/whitespace string,
            //               default(T), empty collection, all fail)
            //
            // Both are stricter-or-equal to "must be present," so both
            // map to IsRequired = true. NotEmpty is the stronger check;
            // if both happen to be applied, keep IsRequired true either way.
            // ============================================================

            case INotEmptyValidator:
            case INotNullValidator:

                model.IsRequired = true;

                break;

            // ============================================================
            // Length
            //
            // Length(min, max)
            // MinimumLength(min)
            // MaximumLength(max)
            // ============================================================

            case ILengthValidator lengthValidator:

                ApplyLength(
                    model,
                    lengthValidator);

                break;

            // ============================================================
            // Regex
            //
            // Matches(...)
            // ============================================================

            case IRegularExpressionValidator regexValidator:

                ApplyRegex(
                    model,
                    regexValidator);

                break;

            // ============================================================
            // Comparisons
            //
            // GreaterThan(...)
            // GreaterThanOrEqualTo(...)
            // LessThan(...)
            // LessThanOrEqualTo(...)
            // ============================================================

            case IComparisonValidator comparisonValidator:

                ApplyComparison(
                    model,
                    comparisonValidator);

                break;

            // ============================================================
            // Range shortcuts
            //
            // InclusiveBetween(min, max)
            // ExclusiveBetween(min, max)
            //
            // FluentValidation implements these as a single validator
            // exposing both bounds, rather than as two IComparisonValidator
            // instances, so they need their own case.
            // ============================================================

            case IBetweenValidator betweenValidator:

                ApplyBetween(
                    model,
                    betweenValidator);

                break;
        }
    }

    protected virtual void ApplyLength(
        PropertyApiDescriptionModel model,
        ILengthValidator validator)
    {
        if (validator.Min > 0)
        {
            model.MinLength =
                model.MinLength.HasValue
                    ? Math.Max(
                        model.MinLength.Value,
                        validator.Min)
                    : validator.Min;
        }

        if (validator.Max > 0)
        {
            model.MaxLength =
                model.MaxLength.HasValue
                    ? Math.Min(
                        model.MaxLength.Value,
                        validator.Max)
                    : validator.Max;
        }
    }

    protected virtual void ApplyRegex(
        PropertyApiDescriptionModel model,
        IRegularExpressionValidator validator)
    {
        if (!string.IsNullOrWhiteSpace(
                validator.Expression))
        {
            model.Regex =
                validator.Expression;
        }
    }

    protected virtual void ApplyComparison(
        PropertyApiDescriptionModel model,
        IComparisonValidator validator)
    {
        var value = FormatComparisonValue(validator.ValueToCompare);

        if (value == null)
        {
            return;
        }

        switch (validator.Comparison)
        {
            case Comparison.GreaterThan:
            case Comparison.GreaterThanOrEqual:

                ApplyMinimum(
                    model,
                    value);

                break;

            case Comparison.LessThan:
            case Comparison.LessThanOrEqual:

                ApplyMaximum(
                    model,
                    value);

                break;
        }
    }

    protected virtual void ApplyBetween(
        PropertyApiDescriptionModel model,
        IBetweenValidator validator)
    {
        var from = FormatComparisonValue(validator.From);
        var to = FormatComparisonValue(validator.To);

        if (from != null)
        {
            ApplyMinimum(
                model,
                from);
        }

        if (to != null)
        {
            ApplyMaximum(
                model,
                to);
        }
    }

    protected virtual string? FormatComparisonValue(object? rawValue)
    {
        if (rawValue == null)
        {
            return null;
        }

        var value = Convert.ToString(
            rawValue,
            CultureInfo.InvariantCulture);

        return string.IsNullOrWhiteSpace(value)
            ? null
            : value;
    }

    protected virtual void ApplyMinimum(
        PropertyApiDescriptionModel model,
        string value)
    {
        if (!decimal.TryParse(
                value,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var minimum))
        {
            return;
        }

        if (decimal.TryParse(
                model.Minimum,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var existingMinimum))
        {
            minimum = Math.Max(
                minimum,
                existingMinimum);
        }

        model.Minimum =
            minimum.ToString(
                CultureInfo.InvariantCulture);
    }

    protected virtual void ApplyMaximum(
        PropertyApiDescriptionModel model,
        string value)
    {
        if (!decimal.TryParse(
                value,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var maximum))
        {
            return;
        }

        if (decimal.TryParse(
                model.Maximum,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var existingMaximum))
        {
            maximum = Math.Min(
                maximum,
                existingMaximum);
        }

        model.Maximum =
            maximum.ToString(
                CultureInfo.InvariantCulture);
    }
}
