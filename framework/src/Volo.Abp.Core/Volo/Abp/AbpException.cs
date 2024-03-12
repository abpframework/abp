using System;

namespace Volo.Abp;

/// <summary>
/// Base exception type for those are thrown by Abp system for Abp specific exceptions.
/// </summary>
public class AbpException : Exception
{
    public AbpException()
    {

    }

    public AbpException(string? message)
        : base(message)
    {

    }

    public AbpException(string? message, Exception? innerException)
        : base(message, innerException)
    {

    }
}
