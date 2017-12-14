namespace Volo.Abp.Validation
{
    public interface IDataAnnotationValidator
    {
        void Validate(object validatingObject);

        void AddErrors(IAbpValidationResult validationResult, object validatingObject);
    }
}