using System;
using System.Collections;

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
        public string Code { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error details.
        /// </summary>
        public string Details { get; set; }

        public IDictionary Data { get; set; }

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
        /// <param name="code">Error code</param>
        /// <param name="details">Error details</param>
        /// <param name="message">Error message</param>
        public RemoteServiceErrorInfo(string message, string details = null, string code = null)
        {
            Message = message;
            Details = details;
            Code = code;
        }
    }
}
