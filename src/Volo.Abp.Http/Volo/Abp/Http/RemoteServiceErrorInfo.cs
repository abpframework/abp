using System;

namespace Volo.Abp.Http
{
    /// <summary>
    /// Used to store information about an error.
    /// </summary>
    [Serializable]
    public class RemoteServiceErrorInfo
    {
        /// <summary>
        /// Error code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error details.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Validation errors if exists.
        /// </summary>
        public RemoteServiceValidationErrorInfo[] ValidationErrors { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="RemoteServiceErrorInfo"/>.
        /// </summary>
        public RemoteServiceErrorInfo()
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="RemoteServiceErrorInfo"/>.
        /// </summary>
        /// <param name="message">Error message</param>
        public RemoteServiceErrorInfo(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Creates a new instance of <see cref="RemoteServiceErrorInfo"/>.
        /// </summary>
        /// <param name="code">Error code</param>
        public RemoteServiceErrorInfo(int code)
        {
            Code = code;
        }

        /// <summary>
        /// Creates a new instance of <see cref="RemoteServiceErrorInfo"/>.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        public RemoteServiceErrorInfo(int code, string message)
            : this(message)
        {
            Code = code;
        }

        /// <summary>
        /// Creates a new instance of <see cref="RemoteServiceErrorInfo"/>.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="details">Error details</param>
        public RemoteServiceErrorInfo(string message, string details)
            : this(message)
        {
            Details = details;
        }

        /// <summary>
        /// Creates a new instance of <see cref="RemoteServiceErrorInfo"/>.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Error message</param>
        /// <param name="details">Error details</param>
        public RemoteServiceErrorInfo(int code, string message, string details)
            : this(message, details)
        {
            Code = code;
        }
    }
}