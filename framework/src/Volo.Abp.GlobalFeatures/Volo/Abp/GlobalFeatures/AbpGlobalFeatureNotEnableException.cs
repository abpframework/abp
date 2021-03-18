using System;
using Volo.Abp.ExceptionHandling;

namespace Volo.Abp.GlobalFeatures
{
    public class AbpGlobalFeatureNotEnableException : AbpException, IHasErrorCode
    {
        public string Code { get; }

        public AbpGlobalFeatureNotEnableException(string message = null, string code = null, Exception innerException = null)
            : base(message, innerException)
        {
            Code = code;
        }

        public AbpGlobalFeatureNotEnableException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
