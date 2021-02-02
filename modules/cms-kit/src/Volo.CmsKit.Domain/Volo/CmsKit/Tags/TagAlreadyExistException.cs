using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Volo.CmsKit.Tags
{
    [Serializable]
    public class TagAlreadyExistException : BusinessException
    {
        public TagAlreadyExistException([NotNull] string entityType, [NotNull] string name)
        {
            Code = CmsKitErrorCodes.Tags.TagAlreadyExist;
            WithData(nameof(Tag.EntityType), entityType);
            WithData(nameof(Tag.Name), name);
        }
    }
}
