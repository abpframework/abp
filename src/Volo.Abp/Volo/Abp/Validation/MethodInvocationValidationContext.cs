using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Volo.Abp.Validation
{
    public class MethodInvocationValidationContext : IAbpValidationResult
    {
        public MethodInfo Method { get; }

        public object[] ParameterValues { get; }

        public ParameterInfo[] Parameters { get; }

        public List<ValidationResult> Errors { get; }

        public List<IShouldNormalize> ObjectsToBeNormalized { get; }

        public MethodInvocationValidationContext(MethodInfo method, object[] parameterValues)
        {
            Method = method;
            ParameterValues = parameterValues;
            Parameters = method.GetParameters();

            Errors = new List<ValidationResult>();
            ObjectsToBeNormalized = new List<IShouldNormalize>();
        }
    }
}