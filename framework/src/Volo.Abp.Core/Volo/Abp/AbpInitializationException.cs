using System;
using System.Runtime.Serialization;

namespace Volo.Abp
{
    public class AbpInitializationException : AbpException
    {
        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// </summary>
        public AbpInitializationException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public AbpInitializationException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public AbpInitializationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// Constructor for serializing.
        /// </summary>
        public AbpInitializationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }
    }
}