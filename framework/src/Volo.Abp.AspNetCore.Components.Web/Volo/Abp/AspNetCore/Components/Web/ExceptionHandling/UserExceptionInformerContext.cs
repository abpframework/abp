using System;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Components.Web.ExceptionHandling
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
