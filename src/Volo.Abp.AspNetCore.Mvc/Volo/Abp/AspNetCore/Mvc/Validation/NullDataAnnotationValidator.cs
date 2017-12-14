using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.Validation
{
    public sealed class NullDataAnnotationValidator : IDataAnnotationValidator
    {
        public static IDataAnnotationValidator Instance { get; } = new DataAnnotationValidator();

        private NullDataAnnotationValidator()
        {
            
        }

        public void AddDataAnnotationAttributeErrors(IAbpValidationResult validationResult, object validatingObject)
        {
            
        }
    }
}