using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.MarkedItems;

public class MarkedItemEntityTypeDefinition : EntityTypeDefinition
{
    public string IconName { get; set; }
    public MarkedItemEntityTypeDefinition(
        [NotNull] string entityType,
        [NotNull] string iconName) : base(entityType)
    {
        Check.NotNull(iconName, nameof(iconName));
        IconName = iconName;
    }
}