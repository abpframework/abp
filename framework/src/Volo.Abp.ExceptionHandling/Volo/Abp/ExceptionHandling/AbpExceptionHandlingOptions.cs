using System;
using System.ComponentModel;

namespace Volo.Abp.ExceptionHandling;

public class AbpExceptionHandlingOptions
{
    public bool IncludeDetails { get; set; } = false;

    public bool IncludeStackTrace { get; set; } = true;

    /// <summary>
    /// Alias to keep Compatibility to previous versions
    /// </summary>
    [Browsable(false)]
    [Obsolete($"Please use '{nameof(IncludeDetails)}' instead.")]
    public bool SendExceptionsDetailsToClients {
        get => IncludeDetails;
        set => IncludeDetails = value;
    }

    /// <summary>
    /// Alias to keep Compatibility to previous versions
    /// </summary>
    [Browsable(false)]
    [Obsolete($"Please use '{nameof(IncludeStackTrace)}' instead.")]
    public bool SendStackTraceToClients {
        get => IncludeStackTrace;
        set => IncludeStackTrace = value;
    }
}
