using System;
using System.Runtime.Serialization;

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

    public AbpInitializationException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }
}
