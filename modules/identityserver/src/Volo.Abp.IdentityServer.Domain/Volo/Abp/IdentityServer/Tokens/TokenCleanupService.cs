using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityServer.Devices;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.Uow;

namespace Volo.Abp.IdentityServer.Tokens
{
    public class TokenCleanupService : ITransientDependency
    {
        protected IPersistentGrantRepository PersistentGrantRepository { get; }
        protected IDeviceFlowCodesRepository DeviceFlowCodesRepository { get; }
        protected TokenCleanupOptions Options { get; }

        public TokenCleanupService(
            IPersistentGrantRepository persistentGrantRepository,
            IDeviceFlowCodesRepository deviceFlowCodesRepository,
            IOptions<TokenCleanupOptions> options)
        {
            PersistentGrantRepository = persistentGrantRepository;
            DeviceFlowCodesRepository = deviceFlowCodesRepository;
            Options = options.Value;
        }

        public virtual async Task CleanAsync()
        {
            await RemoveGrantsAsync();
            await RemoveDeviceCodesAsync();
        }

        [UnitOfWork]
        protected virtual async Task RemoveGrantsAsync()
        {
            for (var i = 0; i < Options.CleanupLoopCount; i++)
            {
                var persistentGrants = await PersistentGrantRepository.GetListByExpirationAsync(DateTime.UtcNow, Options.CleanupBatchSize);

                await PersistentGrantRepository.DeleteManyAsync(persistentGrants);

                //No need to continue to query if it gets more than max items.
                if (persistentGrants.Count < Options.CleanupBatchSize)
                {
                    break;
                }
            }
        }

        protected virtual async Task RemoveDeviceCodesAsync()
        {
            for (var i = 0; i < Options.CleanupLoopCount; i++)
            {
                var deviceFlowCodeses = await DeviceFlowCodesRepository.GetListByExpirationAsync(DateTime.UtcNow, Options.CleanupBatchSize);

                await DeviceFlowCodesRepository.DeleteManyAsync(deviceFlowCodeses);

                //No need to continue to query if it gets more than max items.
                if (deviceFlowCodeses.Count < Options.CleanupBatchSize)
                {
                    break;
                }
            }
        }
    }
}
