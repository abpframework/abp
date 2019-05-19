namespace Volo.Abp.Validation
{
    public interface IObjectValidationContributor
    {
        void AddErrors(ObjectValidationContext context);
    }
}