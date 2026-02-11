using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.MarkedItems;

public class DuplicateMarkedItemDefinitionException : BusinessException
{
    public DuplicateMarkedItemDefinitionException([NotNull] string entityType)
    {
        EntityType = Check.NotNullOrEmpty(entityType, nameof(entityType));
        Code = CmsKitErrorCodes.MarkedItems.DuplicateMarkedItem;
        WithData(nameof(EntityType), EntityType);
    }

    public string EntityType { get; }
}