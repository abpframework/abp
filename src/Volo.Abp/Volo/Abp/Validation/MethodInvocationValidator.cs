namespace Volo.Abp.Validation
{
    /// <summary>
    /// This class is used to validate a method call (invocation) for method arguments.
    /// </summary>
    public class MethodInvocationValidator : MethodInvocationValidatorBase
    {
        public MethodInvocationValidator(IObjectValidator objectValidator) 
            : base(objectValidator)
        {

        }

        public virtual void Validate(MethodInvocationValidationContext context)
        {
            ValidateInternal(context);
        }
    }
}
