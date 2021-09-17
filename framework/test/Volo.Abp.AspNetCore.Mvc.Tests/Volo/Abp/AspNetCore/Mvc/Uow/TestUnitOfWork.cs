using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.UI;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Mvc.Uow
{
    [Dependency(ReplaceServices = true)]
    public class TestUnitOfWork : UnitOfWork
    {
        private readonly TestUnitOfWorkConfig _config;

        public TestUnitOfWork(
            IServiceProvider serviceProvider,
            IUnitOfWorkEventPublisher unitOfWorkEventPublisher,
            IOptions<AbpUnitOfWorkDefaultOptions> options, TestUnitOfWorkConfig config) 
            : base(
                serviceProvider,
                unitOfWorkEventPublisher,
                options)
        {
            _config = config;
        }

        public override Task CompleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowExceptionIfRequested();
            return base.CompleteAsync(cancellationToken);
        }

        private void ThrowExceptionIfRequested()
        {
            if (_config.ThrowExceptionOnComplete)
            {
                throw new UserFriendlyException(TestUnitOfWorkConfig.ExceptionOnCompleteMessage);
            }
        }
    }
}
