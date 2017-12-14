namespace Volo.Abp.Validation
{
    public interface IObjectValidator
    {
        void Validate(object validatingObject, string name = null, bool allowNull = false);

        void AddErrors(IAbpValidationResult validationResult, object validatingObject, string name = null, bool allowNull = false);
    }
}