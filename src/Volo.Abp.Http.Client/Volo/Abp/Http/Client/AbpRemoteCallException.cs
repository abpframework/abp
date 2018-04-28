using System;
using System.Runtime.Serialization;
using Volo.Abp.ExceptionHandling;

namespace Volo.Abp.Http.Client
{
    [Serializable]
    public class AbpRemoteCallException : AbpException, IHasErrorCode, IHasErrorDetails
    {
        public string Code => Error?.Code;

        public string Details => Error?.Details;

        public RemoteServiceErrorInfo Error { get; set; }

        public AbpRemoteCallException()
        {

        }

        public AbpRemoteCallException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        public AbpRemoteCallException(RemoteServiceErrorInfo error)
            : base(error.Message)
        {
            Error = error;
        }
    }
}
