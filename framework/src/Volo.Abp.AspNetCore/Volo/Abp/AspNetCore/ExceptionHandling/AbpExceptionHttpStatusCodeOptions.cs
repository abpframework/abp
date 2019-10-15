using System.Collections.Generic;
using System.Net;

namespace Volo.Abp.AspNetCore.ExceptionHandling
{
    public class AbpExceptionHttpStatusCodeOptions
    {
        public IDictionary<string, HttpStatusCode> ErrorCodeToHttpStatusCodeMappings { get; }

        public AbpExceptionHttpStatusCodeOptions()
        {
            ErrorCodeToHttpStatusCodeMappings = new Dictionary<string, HttpStatusCode>();
        }

        public void Map(string errorCode, HttpStatusCode httpStatusCode)
        {
            ErrorCodeToHttpStatusCodeMappings[errorCode] = httpStatusCode;
        }
    }
}