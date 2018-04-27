using System;
using Microsoft.Extensions.Logging;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Logging;

namespace Volo.Abp
{
    public class BusinessException : Exception, IBusinessException, IHasErrorDetails, IHasLogLevel
    {
        public string Code { get; set; }

        public string Details { get; set; }

        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        public override string Message => base.Message ?? Code;

        public BusinessException()
        {
            
        }

        public BusinessException(string code, string message = null, string details = null)
            : base(message)
        {
            Code = code;
            Details = details;
        }
    }
}