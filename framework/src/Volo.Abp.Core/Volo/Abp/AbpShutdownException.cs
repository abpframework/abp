using System;
using System.Runtime.Serialization;

namespace Volo.Abp;

public class AbpShutdownException : AbpException
{
    public AbpShutdownException()
    {

    }

    public AbpShutdownException(string message)
        : base(message)
    {

    }

    public AbpShutdownException(string message, Exception innerException)
        : base(message, innerException)
    {

    }

    public AbpShutdownException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }
}
