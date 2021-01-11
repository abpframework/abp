using System;
using Volo.Abp;

namespace Volo.CmsKit.Tags
{
    [Serializable]
    public class EntityNotTaggableException : BusinessException
    {
        public EntityNotTaggableException(string code = null, string message = null, string details = null, Exception innerException = null, Microsoft.Extensions.Logging.LogLevel logLevel = Microsoft.Extensions.Logging.LogLevel.Warning) : base(code, message, details, innerException, logLevel)
        {
        }

        public EntityNotTaggableException(string entityType) 
        {
            Code = CmsKitErrorCodes.EntityNotTaggable;
            WithData(nameof(Tag.EntityType), entityType);
        }
    }
}
