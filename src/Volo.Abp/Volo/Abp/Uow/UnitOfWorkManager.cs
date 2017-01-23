using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Volo.DependencyInjection;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkManager : IUnitOfWorkManager, ISingletonDependency
    {
        public IUnitOfWork Current => _ambientUnitOfWork.UnitOfWork;

        private readonly IServiceProvider _serviceProvider;
        private readonly IAmbientUnitOfWork _ambientUnitOfWork;

        public UnitOfWorkManager(IServiceProvider serviceProvider, IAmbientUnitOfWork ambientUnitOfWork)
        {
            _serviceProvider = serviceProvider;
            _ambientUnitOfWork = ambientUnitOfWork;
        }

        public IBasicUnitOfWork Begin()
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

            Debug.Assert(_ambientUnitOfWork.UnitOfWork != null, "_ambientUnitOfWork.UnitOfWork can not be null since it's set by _ambientUnitOfWork.SetUnitOfWork method!");
            return _ambientUnitOfWork.UnitOfWork;
        }
    }
}