using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.MarkedItems;

public class MarkedItemDefinitionNotFoundException : BusinessException
{
    public MarkedItemDefinitionNotFoundException([NotNull] string entityType)
    {
        EntityType = Check.NotNullOrEmpty(entityType, nameof(entityType));
        Code = CmsKitErrorCodes.MarkedItems.MarkedItemDefinitionNotFound;
        WithData(nameof(EntityType), EntityType);
    }

    public string EntityType { get; }
}
