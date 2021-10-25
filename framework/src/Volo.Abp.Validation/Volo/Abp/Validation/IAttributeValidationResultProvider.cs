using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Validation
{
    public interface IAttributeValidationResultProvider
    {
        ValidationResult GetOrDefault(ValidationAttribute validationAttribute, object validatingObject, ValidationContext validationContext);
    }
}
