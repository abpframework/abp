using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.SecurityLog;
using Volo.Abp.Uow;

namespace Volo.Abp.Identity
{
    [Dependency(ReplaceServices = true)]
    public class IdentitySecurityLogStore : ISecurityLogStore, ITransientDependency
    {
        public ILogger<IdentitySecurityLogStore> Logger { get; set; }

        protected AbpSecurityLogOptions SecurityLogOptions { get; }
        protected IIdentitySecurityLogRepository IdentitySecurityLogRepository { get; }
        protected IGuidGenerator GuidGenerator { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }

        public IdentitySecurityLogStore(
            ILogger<IdentitySecurityLogStore> logger,
            IOptions<AbpSecurityLogOptions> securityLogOptions,
            IIdentitySecurityLogRepository identitySecurityLogRepository,
            IGuidGenerator guidGenerator,
            IUnitOfWorkManager unitOfWorkManager)
        {
            Logger = logger;
            SecurityLogOptions = securityLogOptions.Value;
            IdentitySecurityLogRepository = identitySecurityLogRepository;
            GuidGenerator = guidGenerator;
            UnitOfWorkManager = unitOfWorkManager;
        }

        public async Task SaveAsync(SecurityLogInfo securityLogInfo)
        {
            if (!SecurityLogOptions.IsEnabled)
            {
                return;
            }

            using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
            {
                await IdentitySecurityLogRepository.InsertAsync(new IdentitySecurityLog(GuidGenerator, securityLogInfo));
                await uow.CompleteAsync();
            }
        }
    }
}
