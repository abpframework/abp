using System;

namespace Volo.Abp.ExceptionHandling;

/// <summary>
/// Used to store information about a validation error.
/// </summary>
[Serializable]
public class ValidationErrorInfo
{
    /// <summary>
    /// Validation error message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Relate invalid members (fields/properties).
    /// </summary>
    public string[] Members { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="ValidationErrorInfo"/>.
    /// </summary>
    public ValidationErrorInfo()
    {

    }

    /// <summary>
    /// Creates a new instance of <see cref="ValidationErrorInfo"/>.
    /// </summary>
    /// <param name="message">Validation error message</param>
    public ValidationErrorInfo(string message)
    {
        Message = message;
    }

    /// <summary>
    /// Creates a new instance of <see cref="ValidationErrorInfo"/>.
    /// </summary>
    /// <param name="message">Validation error message</param>
    /// <param name="members">Related invalid members</param>
    public ValidationErrorInfo(string message, string[] members)
        : this(message)
    {
        Members = members;
    }

    /// <summary>
    /// Creates a new instance of <see cref="ValidationErrorInfo"/>.
    /// </summary>
    /// <param name="message">Validation error message</param>
    /// <param name="member">Related invalid member</param>
    public ValidationErrorInfo(string message, string member)
        : this(message, new[] { member })
    {

    }
}
