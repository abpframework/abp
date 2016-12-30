using System.Threading;
using JetBrains.Annotations;
using Volo.Abp.Threading;

namespace Volo.Abp.Uow
{
    public static class UnitOfWorkExtensions
    {
        //TODO: Implement all sync versions

        public static void Complete([NotNull] this IUnitOfWork unitOfWork, CancellationToken cancellationToken = default(CancellationToken))
        {
            Check.NotNull(unitOfWork, nameof(unitOfWork));

            AsyncHelper.RunSync(() => unitOfWork.CompleteAsync(cancellationToken));
        }
    }
}