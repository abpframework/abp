using System;
using System.Runtime.Serialization;

namespace Volo.Abp.Http.Client
{
    [Serializable]
    public class AbpRemoteCallException : AbpException
    {
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
