using System;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Logging;

namespace Volo.Abp.Authorization
{
    /// <summary>
    /// This exception is thrown on an unauthorized request.
    /// </summary>
    [Serializable]
    public class AbpAuthorizationException : AbpException, IHasLogLevel, IHasErrorCode
    {
        /// <summary>
        /// Severity of the exception.
        /// Default: Warn.
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Error code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Creates a new <see cref="AbpAuthorizationException"/> object.
        /// </summary>
        public AbpAuthorizationException()
        {
            LogLevel = LogLevel.Warning;
        }

        /// <summary>
        /// Creates a new <see cref="AbpAuthorizationException"/> object.
        /// </summary>
        public AbpAuthorizationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpAuthorizationException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public AbpAuthorizationException(string message)
            : base(message)
        {
            LogLevel = LogLevel.Warning;
        }

        /// <summary>
        /// Creates a new <see cref="AbpAuthorizationException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public AbpAuthorizationException(string message, Exception innerException)
            : base(message, innerException)
        {
            LogLevel = LogLevel.Warning;
        }

        /// <summary>
        /// Creates a new <see cref="AbpAuthorizationException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="code">Exception code</param>
        /// <param name="innerException">Inner exception</param>
        public AbpAuthorizationException(string message = null, string code = null, Exception innerException = null)
            : base(message, innerException)
        {
            Code = code;
            LogLevel = LogLevel.Warning;
        }

        public AbpAuthorizationException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
