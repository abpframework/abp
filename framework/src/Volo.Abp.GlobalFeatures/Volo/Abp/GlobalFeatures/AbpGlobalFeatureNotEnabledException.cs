using System;
using Volo.Abp.ExceptionHandling;

namespace Volo.Abp.GlobalFeatures
{
    [Serializable]
    public class AbpGlobalFeatureNotEnabledException : AbpException, IHasErrorCode
    {
        public string Code { get; }

        public AbpGlobalFeatureNotEnabledException(string message = null, string code = null, Exception innerException = null)
            : base(message, innerException)
        {
            Code = code;
        }

        public AbpGlobalFeatureNotEnabledException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
