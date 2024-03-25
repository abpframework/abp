namespace Volo.Abp.Domain.Entities.Events;

public class AbpEntityChangeOptions
{
    /// <summary>
    /// Default: true.
    /// Publish the EntityUpdatedEvent when any navigation property changes.
    /// </summary>
    public bool PublishEntityUpdatedEventWhenNavigationChanges { get; set; } = true;
}
