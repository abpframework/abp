using System;
using System.Collections;
using Volo.Abp.Http;

namespace Volo.Abp.ExceptionHandling;

/// <summary>
/// Used to store information about an error.
/// </summary>
[Serializable]
public class ErrorInfo
{
    /// <summary>
    /// Error code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Error message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Error details.
    /// </summary>
    public string Details { get; set; }

    /// <summary>
    /// Error data.
    /// </summary>
    public IDictionary Data { get; set; }

    /// <summary>
    /// Validation errors if exists.
    /// </summary>
    public ValidationErrorInfo[] ValidationErrors { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="ErrorInfo"/>.
    /// </summary>
    public ErrorInfo()
    {

    }

    /// <summary>
    /// Creates a new instance of <see cref="ErrorInfo"/>.
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="details">Error details</param>
    /// <param name="message">Error message</param>
    /// <param name="data">Error data</param>
    public ErrorInfo(string message, string details = null, string code = null, IDictionary data = null)
    {
        Message = message;
        Details = details;
        Code = code;
        Data = data;
    }
}
