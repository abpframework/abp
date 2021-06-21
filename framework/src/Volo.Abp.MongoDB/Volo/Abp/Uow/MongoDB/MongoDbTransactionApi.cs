using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Volo.Abp.Threading;

namespace Volo.Abp.Uow.MongoDB
{
    public class MongoDbTransactionApi : ITransactionApi, ISupportsRollback
    {
        public IClientSessionHandle SessionHandle { get; }

        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        public MongoDbTransactionApi(
            IClientSessionHandle sessionHandle,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            SessionHandle = sessionHandle;
            CancellationTokenProvider = cancellationTokenProvider;
        }

        public async Task CommitAsync()
        {
            await SessionHandle.CommitTransactionAsync(CancellationTokenProvider.Token);
        }

        public void Dispose()
        {
            SessionHandle.Dispose();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            await SessionHandle.AbortTransactionAsync(
                CancellationTokenProvider.FallbackToProvider(cancellationToken)
            );
        }
    }
}
