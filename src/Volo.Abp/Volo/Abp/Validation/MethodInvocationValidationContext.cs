using System.Reflection;

namespace Volo.Abp.Validation
{
    public class MethodInvocationValidationContext : AbpValidationResult
    {
        public MethodInfo Method { get; }

        public object[] ParameterValues { get; }

        public ParameterInfo[] Parameters { get; }

        public MethodInvocationValidationContext(MethodInfo method, object[] parameterValues)
        {
            Method = method;
            ParameterValues = parameterValues;
            Parameters = method.GetParameters();
        }
    }
}