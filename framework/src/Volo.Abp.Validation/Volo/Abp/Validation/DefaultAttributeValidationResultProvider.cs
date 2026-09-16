using System.ComponentModel.DataAnnotations;
using Volo.Abp.DependencyInjection;
#if NET11_0_OR_GREATER
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Volo.Abp.Validation;

public class DefaultAttributeValidationResultProvider : IAttributeValidationResultProvider, ITransientDependency
{
    public virtual ValidationResult? GetOrDefault(ValidationAttribute validationAttribute, object? validatingObject, ValidationContext validationContext)
    {
        return validationAttribute.GetValidationResult(validatingObject, validationContext);
    }

#if NET11_0_OR_GREATER
    public virtual Task<ValidationResult?> GetOrDefaultAsync(
        ValidationAttribute validationAttribute,
        object? validatingObject,
        ValidationContext validationContext,
        CancellationToken cancellationToken = default)
    {
        if (validationAttribute is AsyncValidationAttribute asyncValidationAttribute)
        {
            return asyncValidationAttribute.GetValidationResultAsync(validatingObject, validationContext, cancellationToken);
        }

        return Task.FromResult(GetOrDefault(validationAttribute, validatingObject, validationContext));
    }
#endif
}
