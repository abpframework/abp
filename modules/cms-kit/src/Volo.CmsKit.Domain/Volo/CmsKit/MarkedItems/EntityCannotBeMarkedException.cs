using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.MarkedItems;

public class EntityCannotBeMarkedException : BusinessException
{
    public EntityCannotBeMarkedException([NotNull] string entityType)
    {
        EntityType = Check.NotNullOrEmpty(entityType, nameof(entityType));
        Code = CmsKitErrorCodes.MarkedItems.EntityCannotBeMarked;
        WithData(nameof(EntityType), EntityType);
    }

    public string EntityType { get; }
}
