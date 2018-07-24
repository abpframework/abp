using System;
using System.Runtime.Serialization;

namespace Volo.Abp.BackgroundJobs
{
    [Serializable]
    public class BackgroundJobException : AbpException
    {
        public string JobName { get; set; }

        public string JobArgs { get; set; }

        public BackgroundJobException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="BackgroundJobException"/> object.
        /// </summary>
        public BackgroundJobException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="BackgroundJobException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public BackgroundJobException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
