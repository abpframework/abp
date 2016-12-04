using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Volo.CodeAnnotations
{
    //TODO: Remove this library and move Check to somewhere else!

    [DebuggerStepThrough]
    public static class Check
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(T value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }
    }
}
