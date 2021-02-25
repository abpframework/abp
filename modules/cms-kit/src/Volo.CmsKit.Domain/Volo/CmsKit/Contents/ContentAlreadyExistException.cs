using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Volo.CmsKit.Contents
{
    [Serializable]
    public class ContentAlreadyExistException : BusinessException
    {
        public ContentAlreadyExistException([NotNull] string entityType, [NotNull] string entityId)
        {
            Code = CmsKitErrorCodes.ContentAlreadyExist;
            WithData(nameof(Content.EntityType), entityType);
            WithData(nameof(Content.EntityId), entityId);
        }
    }
}
