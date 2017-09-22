using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    public interface IUnitOfWorkManager
    {
        [CanBeNull]
        IUnitOfWork Current { get; }

        [NotNull]
        IBasicUnitOfWork Begin([NotNull] UnitOfWorkStartOptions options);
    }

    public static class UnitOfWorkManagerExtensions
    {
        [NotNull]
        public static IBasicUnitOfWork Begin(this IUnitOfWorkManager unitOfWorkManager)
        {
            return unitOfWorkManager.Begin(new UnitOfWorkStartOptions());
        }

        [NotNull]
        public static IBasicUnitOfWork BeginNew(this IUnitOfWorkManager unitOfWorkManager)
        {
            return unitOfWorkManager.Begin(new UnitOfWorkStartOptions {RequiresNew = true});
        }
    }
}