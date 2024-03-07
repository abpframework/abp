using System;

namespace Volo.Abp;

public class AbpInitializationException : AbpException
{
    public AbpInitializationException()
    {

    }

    public AbpInitializationException(string message)
        : base(message)
    {

    }

    public AbpInitializationException(string message, Exception innerException)
        : base(message, innerException)
    {

    }
}
