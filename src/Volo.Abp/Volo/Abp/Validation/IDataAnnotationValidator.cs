namespace Volo.Abp.Validation
{
    public interface IDataAnnotationValidator
    {
        void AddDataAnnotationAttributeErrors(IAbpValidationResult validationResult, object validatingObject);
    }
}