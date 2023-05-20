namespace Volo.Abp.Domain.Entities.Events.Distributed;

public interface IEntityEto<TKey>
{
    /// <summary>
    /// Unique identifier for this entity.
    /// </summary>
    TKey Id { get; set; }
}