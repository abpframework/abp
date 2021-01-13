using System;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Components.UI.ExceptionHandling
{
    public class UserExceptionInformerContext
    {
        [NotNull]
        public Exception Exception { get; }

        public UserExceptionInformerContext(Exception exception)
        {
            Exception = exception;
        }
    }
}
