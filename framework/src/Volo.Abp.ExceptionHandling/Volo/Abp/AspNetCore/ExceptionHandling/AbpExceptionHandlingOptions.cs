using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.ExceptionHandling;

public class AbpExceptionHandlingOptions
{
    public bool SendExceptionsDetailsToClients { get; set; }

    public bool SendStackTraceToClients { get; set; }

    public List<Type> SendExceptionDataToClientTypes { get; set; }

    public AbpExceptionHandlingOptions()
    {
        SendExceptionsDetailsToClients = false;
        SendStackTraceToClients = true;
        SendExceptionDataToClientTypes =
        [
            typeof(IBusinessException)
        ];
    }
}
