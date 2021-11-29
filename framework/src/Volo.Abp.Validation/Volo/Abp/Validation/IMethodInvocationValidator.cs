using System.Threading.Tasks;

namespace Volo.Abp.Validation
{
    public interface IMethodInvocationValidator
    {
        Task ValidateAsync(MethodInvocationValidationContext context);
    }
}
