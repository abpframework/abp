namespace Volo.Abp.Validation
{
    public interface IObjectValidator
    {
        void Validate(IAbpValidationResult validationResult, object validatingObject);

        void AddValidatationErrors(IAbpValidationResult validationResult, object validatingObject);
    }
}