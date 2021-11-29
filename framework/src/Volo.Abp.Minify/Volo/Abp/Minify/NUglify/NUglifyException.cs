using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NUglify;

namespace Volo.Abp.Minify.NUglify
{
    public class NUglifyException : AbpException
    {
        public List<UglifyError> Errors { get; set; }

        public NUglifyException(string message, List<UglifyError> errors)
            : base(message)
        {
            Errors = errors;
        }

        public NUglifyException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// Constructor for serializing.
        /// </summary>
        public NUglifyException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }
    }
}
