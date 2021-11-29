using System;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Studio
{
    public class AbpStudioException : BusinessException
    {
        public AbpStudioException(
            string code = null, 
            string message = null, 
            string details = null, 
            Exception innerException = null,
            LogLevel logLevel = LogLevel.Warning)
            : base(code, message, details, innerException, logLevel)
        {
            Code = code;
            Details = details;
            LogLevel = logLevel;
        }

        public AbpStudioException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }
    }
}