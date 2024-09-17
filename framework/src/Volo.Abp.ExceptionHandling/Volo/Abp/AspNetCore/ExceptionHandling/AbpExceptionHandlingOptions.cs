using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.ExceptionHandling;

public class AbpExceptionHandlingOptions
{
    public bool SendExceptionsDetailsToClients { get; set; } = false;

    public bool SendStackTraceToClients { get; set; } = true;

    public List<Type> SendExceptionDataToClientTypes { get; } = new List<Type>();
}
