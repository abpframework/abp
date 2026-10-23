using System.ComponentModel.DataAnnotations;
#if NET11_0_OR_GREATER
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Volo.Abp.Validation;

public interface IAttributeValidationResultProvider
{
    ValidationResult? GetOrDefault(ValidationAttribute validationAttribute, object? validatingObject, ValidationContext validationContext);

#if NET11_0_OR_GREATER
    /* AsyncValidationAttribute seals IsValid and routes it through the synchronous overload, which
     * attributes that only validate asynchronously are expected to throw from. They have to be
     * awaited here instead. The default implementation keeps existing providers working. */
    Task<ValidationResult?> GetOrDefaultAsync(
        ValidationAttribute validationAttribute,
        object? validatingObject,
        ValidationContext validationContext,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(GetOrDefault(validationAttribute, validatingObject, validationContext));
    }
#endif
}
