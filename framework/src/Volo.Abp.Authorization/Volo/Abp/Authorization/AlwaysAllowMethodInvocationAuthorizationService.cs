using System.Threading.Tasks;

namespace Volo.Abp.Authorization
{
    public class AlwaysAllowMethodInvocationAuthorizationService : IMethodInvocationAuthorizationService
    {
        public Task CheckAsync(MethodInvocationAuthorizationContext context)
        {
            return Task.CompletedTask;
        }
    }
}