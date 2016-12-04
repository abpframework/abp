using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Volo
{
    //TODO: This code should not be here and this library should not depend on JetBrains.Annotations.

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
