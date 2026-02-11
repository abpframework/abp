using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.AspNetCore.ExceptionHandling;

public class AbpExceptionHandlingOptions
{
    public bool SendExceptionsDetailsToClients { get; set; }

    public bool SendStackTraceToClients { get; set; }

    public List<Type> SendExceptionDataToClientTypes { get; set; }

    /// <summary>
    /// Selectors to exclude exception from logging.
    /// If a selector returns true, the exception is not logged in to the logging.
    /// </summary>
    public List<Func<Exception, bool>> ExcludeExceptionFromLoggerSelectors { get; }

    public AbpExceptionHandlingOptions()
    {
        SendExceptionsDetailsToClients = false;
        SendStackTraceToClients = true;
        SendExceptionDataToClientTypes =
        [
            typeof(IBusinessException)
        ];
        ExcludeExceptionFromLoggerSelectors = new List<Func<Exception, bool>>();
    }

    public bool ShouldLogException(Exception exception)
    {
        return ExcludeExceptionFromLoggerSelectors.All(selector => !selector(exception));
    }
}
