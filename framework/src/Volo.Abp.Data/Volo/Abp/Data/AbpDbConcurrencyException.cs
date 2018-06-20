using System;

namespace Volo.Abp.Data
{
    public class AbpDbConcurrencyException : AbpException
    {
        /// <summary>
        /// Creates a new <see cref="AbpDbConcurrencyException"/> object.
        /// </summary>
        public AbpDbConcurrencyException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpDbConcurrencyException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public AbpDbConcurrencyException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpDbConcurrencyException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public AbpDbConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}