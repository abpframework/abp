using System;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;
using Volo.Abp.Logging;

namespace Volo.Abp.Ui
{
    /// <summary>
    /// This exception type is directly shown to the user.
    /// </summary>
    [Serializable]
    public class UserFriendlyException : AbpException, IHasLogLevel, IHasErrorCode, IUserFriendlyException
    {
        /// <summary>
        /// Additional information about the exception.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// An arbitrary error code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Severity of the exception.
        /// Default: Warn.
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserFriendlyException()
        {
            LogLevel = LogLevel.Warning;
        }

        /// <summary>
        /// Constructor for serializing.
        /// </summary>
        public UserFriendlyException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public UserFriendlyException(string message)
            : base(message)
        {
            LogLevel = LogLevel.Warning;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="logLevel">Exception severity</param>
        public UserFriendlyException(string message, LogLevel logLevel)
            : base(message)
        {
            LogLevel = logLevel;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Exception message</param>
        public UserFriendlyException(int code, string message)
            : this(message)
        {
            Code = code;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="details">Additional information about the exception</param>
        public UserFriendlyException(string message, string details)
            : this(message)
        {
            Details = details;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Exception message</param>
        /// <param name="details">Additional information about the exception</param>
        public UserFriendlyException(int code, string message, string details)
            : this(message, details)
        {
            Code = code;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public UserFriendlyException(string message, Exception innerException)
            : base(message, innerException)
        {
            LogLevel = LogLevel.Warning;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="details">Additional information about the exception</param>
        /// <param name="innerException">Inner exception</param>
        public UserFriendlyException(string message, string details, Exception innerException)
            : this(message, innerException)
        {
            Details = details;
        }
    }
}