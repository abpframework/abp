using System.Threading.Tasks;

namespace Volo.Abp.Authorization
{
    public interface IMethodInvocationAuthorizationService
    {
        Task CheckAsync(MethodInvocationAuthorizationContext context);
    }
}