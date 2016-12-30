using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.DependencyInjection;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkManager : IUnitOfWorkManager, ISingletonDependency
    {
        //TODO: Skipped many feature of Abp 1.x
        //TODO: Inner, real unit of works (RequiresNew option)!

        public IUnitOfWork Current => _ambientUnitOfWork.UnitOfWork; //TODO: Remove Current!

        private readonly IServiceProvider _serviceProvider;
        private readonly IAmbientUnitOfWork _ambientUnitOfWork;

        public UnitOfWorkManager(IServiceProvider serviceProvider, IAmbientUnitOfWork ambientUnitOfWork)
        {
            _serviceProvider = serviceProvider;
            _ambientUnitOfWork = ambientUnitOfWork;
        }

        public IUnitOfWork Begin()
        {
            if (_ambientUnitOfWork.UnitOfWork != null)
            {
                return new ChildUnitOfWork(_ambientUnitOfWork.UnitOfWork);
            }

            var scope = _serviceProvider.CreateScope();
            try
            {
                _ambientUnitOfWork.SetUnitOfWork(scope.ServiceProvider.GetRequiredService<IUnitOfWork>());
            }
            catch
            {
                scope.Dispose();
                throw;
            }

            return _ambientUnitOfWork.UnitOfWork;
        }
    }
}