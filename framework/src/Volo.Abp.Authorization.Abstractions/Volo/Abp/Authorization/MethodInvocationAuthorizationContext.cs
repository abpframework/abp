using System.Reflection;

namespace Volo.Abp.Authorization
{
    public class MethodInvocationAuthorizationContext
    {
        public MethodInfo Method { get; }

        public MethodInvocationAuthorizationContext(MethodInfo method)
        {
            Method = method;
        }
    }
}