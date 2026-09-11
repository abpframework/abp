using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

[Dependency(ReplaceServices = true)]
public class AbpMvcAttributeValidationResultProvider : DefaultAttributeValidationResultProvider
{
    private readonly AbpMvcDataAnnotationsLocalizationOptions _abpMvcDataAnnotationsLocalizationOptions;
    private readonly IStringLocalizerFactory _stringLocalizerFactory;

    public AbpMvcAttributeValidationResultProvider(
        IOptions<AbpMvcDataAnnotationsLocalizationOptions> abpMvcDataAnnotationsLocalizationOptions,
        IStringLocalizerFactory stringLocalizerFactory)
    {
        _abpMvcDataAnnotationsLocalizationOptions = abpMvcDataAnnotationsLocalizationOptions.Value;
        _stringLocalizerFactory = stringLocalizerFactory;
    }

    public override ValidationResult? GetOrDefault(ValidationAttribute validationAttribute, object? validatingObject, ValidationContext validationContext)
    {
        LocalizeErrorMessage(validationAttribute, validationContext);
        return base.GetOrDefault(validationAttribute, validatingObject, validationContext);
    }

    public override Task<ValidationResult?> GetOrDefaultAsync(
        ValidationAttribute validationAttribute,
        object? validatingObject,
        ValidationContext validationContext,
        CancellationToken cancellationToken = default)
    {
        LocalizeErrorMessage(validationAttribute, validationContext);
        return base.GetOrDefaultAsync(validationAttribute, validatingObject, validationContext, cancellationToken);
    }

    protected virtual void LocalizeErrorMessage(ValidationAttribute validationAttribute, ValidationContext validationContext)
    {
        var resourceSource = _abpMvcDataAnnotationsLocalizationOptions.AssemblyResources.GetOrDefault(validationContext.ObjectType.Assembly);
        if (resourceSource == null)
        {
            return;
        }

        if (validationAttribute.ErrorMessage == null)
        {
            ValidationAttributeHelper.SetDefaultErrorMessage(validationAttribute);
        }

        if (validationAttribute.ErrorMessage != null)
        {
            validationAttribute.ErrorMessage = _stringLocalizerFactory.Create(resourceSource)[validationAttribute.ErrorMessage];
        }
    }
}
