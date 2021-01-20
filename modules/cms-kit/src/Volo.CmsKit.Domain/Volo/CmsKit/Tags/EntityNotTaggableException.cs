using Microsoft.Extensions.Logging;
using System;
using Volo.Abp;

namespace Volo.CmsKit.Tags
{
    [Serializable]
    public class EntityNotTaggableException : BusinessException
    {
        public EntityNotTaggableException(
            string code = null,
            string message = null,
            string details = null,
            Exception innerException = null,
            LogLevel logLevel = LogLevel.Warning) 
            : base(code, message, details, innerException, logLevel)
        {
        }

        public EntityNotTaggableException(string entityType) 
        {
            Code = CmsKitErrorCodes.Tags.EntityNotTaggable;
            WithData(nameof(Tag.EntityType), entityType);
        }
    }
}
