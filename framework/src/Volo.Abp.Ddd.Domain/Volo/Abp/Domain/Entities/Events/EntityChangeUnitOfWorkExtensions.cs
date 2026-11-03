using System;
using JetBrains.Annotations;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Entities.Events;

public static class EntityChangeUnitOfWorkExtensions
{
    private const string UpdateAggregateRootWhenNavigationChangesItemKey = "Abp.UpdateAggregateRootWhenNavigationChanges";

    /// <summary>
    /// Disables updating the aggregate root when one of its navigation properties changes,
    /// for the given unit of work. It overrides the <see cref="AbpEntityChangeOptions.UpdateAggregateRootWhenNavigationChanges"/> option.
    /// It doesn't disable publishing the entity updated event, which is still controlled by the
    /// <see cref="AbpEntityChangeOptions.PublishEntityUpdatedEventWhenNavigationChanges"/> option
    /// and the <see cref="AbpEntityChangeOptions.IgnoredNavigationEntitySelectors"/>.
    /// </summary>
    /// <param name="unitOfWork">A unit of work object</param>
    /// <returns>
    /// A disposable object. Dispose it to restore the setting back to its previous state.
    /// </returns>
    public static IDisposable DisableUpdateAggregateRootWhenNavigationChanges([NotNull] this IUnitOfWork unitOfWork)
    {
        return SetUpdateAggregateRootWhenNavigationChanges(unitOfWork, false);
    }

    /// <summary>
    /// Enables updating the aggregate root when one of its navigation properties changes,
    /// for the given unit of work. It overrides the <see cref="AbpEntityChangeOptions.UpdateAggregateRootWhenNavigationChanges"/> option.
    /// </summary>
    /// <param name="unitOfWork">A unit of work object</param>
    /// <returns>
    /// A disposable object. Dispose it to restore the setting back to its previous state.
    /// </returns>
    public static IDisposable EnableUpdateAggregateRootWhenNavigationChanges([NotNull] this IUnitOfWork unitOfWork)
    {
        return SetUpdateAggregateRootWhenNavigationChanges(unitOfWork, true);
    }

    /// <summary>
    /// Returns the value set for the given unit of work, or null when it was not set.
    /// The <see cref="AbpEntityChangeOptions.UpdateAggregateRootWhenNavigationChanges"/> option is used when it is null.
    /// </summary>
    public static bool? GetUpdateAggregateRootWhenNavigationChangesOrNull([NotNull] this IUnitOfWork unitOfWork)
    {
        Check.NotNull(unitOfWork, nameof(unitOfWork));

        return unitOfWork.Items.TryGetValue(UpdateAggregateRootWhenNavigationChangesItemKey, out var value)
            ? value as bool?
            : null;
    }

    private static IDisposable SetUpdateAggregateRootWhenNavigationChanges(IUnitOfWork unitOfWork, bool value)
    {
        Check.NotNull(unitOfWork, nameof(unitOfWork));

        // Items of a child unit of work is the same instance with its parent,
        // so the previous value is restored on dispose to not leak into the outer unit of work.
        var previousValue = unitOfWork.GetUpdateAggregateRootWhenNavigationChangesOrNull();
        unitOfWork.Items[UpdateAggregateRootWhenNavigationChangesItemKey] = value;

        return new DisposeAction(() =>
        {
            if (previousValue == null)
            {
                unitOfWork.Items.Remove(UpdateAggregateRootWhenNavigationChangesItemKey);
            }
            else
            {
                unitOfWork.Items[UpdateAggregateRootWhenNavigationChangesItemKey] = previousValue.Value;
            }
        });
    }
}
