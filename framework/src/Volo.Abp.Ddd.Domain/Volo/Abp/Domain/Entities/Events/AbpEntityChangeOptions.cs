namespace Volo.Abp.Domain.Entities.Events;

public class AbpEntityChangeOptions
{
    /// <summary>
    /// Default: true.
    /// Publish the EntityUpdatedEvent when any navigation property changes.
    /// </summary>
    public bool PublishEntityUpdatedEventWhenNavigationChanges { get; set; } = true;

    public IEntitySelectorList IgnoredNavigationEntitySelectors { get; set; }

    /// <summary>
    /// Default: true.
    /// Update the aggregate root when any navigation property changes.
    /// Some properties like ConcurrencyStamp,LastModificationTime,LastModifierId etc. will be updated.
    /// </summary>
    public bool UpdateAggregateRootWhenNavigationChanges { get; set; } = true;

    public IEntitySelectorList IgnoredUpdateAggregateRootSelectors { get; set; }

    public AbpEntityChangeOptions()
    {
        IgnoredNavigationEntitySelectors = new EntitySelectorList();
        IgnoredUpdateAggregateRootSelectors = new EntitySelectorList();
    }
}
