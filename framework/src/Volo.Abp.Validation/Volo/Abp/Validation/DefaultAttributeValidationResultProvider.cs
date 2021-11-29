using System.ComponentModel.DataAnnotations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Validation
{
    public class DefaultAttributeValidationResultProvider : IAttributeValidationResultProvider, ITransientDependency
    {
        public virtual ValidationResult GetOrDefault(ValidationAttribute validationAttribute, object validatingObject, ValidationContext validationContext)
        {
            return validationAttribute.GetValidationResult(validatingObject, validationContext);
        }
    }
}
