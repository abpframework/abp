using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Volo.DependencyInjection;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkManager : IUnitOfWorkManager, ISingletonDependency
    {
        //TODO: Skipped many feature of Abp 1.x

        public IUnitOfWork Current => _currentUowInfo.Value?.UnitOfWork;

        private readonly AsyncLocal<UnitOfWorkInfo> _currentUowInfo;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _currentUowInfo = new AsyncLocal<UnitOfWorkInfo>();
        }

        public IUnitOfWork Begin()
        {
            if (Current != null)
            {
                return new ChildUnitOfWork(Current);
            }

            var scope = _serviceProvider.CreateScope();

            try
            {
                _currentUowInfo.Value = new UnitOfWorkInfo(
                    scope.ServiceProvider.GetRequiredService<IUnitOfWork>(),
                    scope
                );
            }
            catch
            {
                scope.Dispose();
                throw;
            }

            return Current;
        }
    }
}