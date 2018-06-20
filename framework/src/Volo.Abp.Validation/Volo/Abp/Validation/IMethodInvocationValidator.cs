namespace Volo.Abp.Validation
{
    public interface IMethodInvocationValidator
    {
        void Validate(MethodInvocationValidationContext context);
    }
}