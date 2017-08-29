using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

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

            var parentUow = _ambientUnitOfWork.UnitOfWork;

            var scope = _serviceProvider.CreateScope();
            IUnitOfWork unitOfWork;
            try
            {
                unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            }
            catch
            {
                scope.Dispose();
                throw;
            }

            _ambientUnitOfWork.SetUnitOfWork(unitOfWork);

            Debug.Assert(
                _ambientUnitOfWork.UnitOfWork != null,
                "_ambientUnitOfWork.UnitOfWork can not be null since it's set by _ambientUnitOfWork.SetUnitOfWork method!"
            );

            unitOfWork.Completed += (sender, args) =>
            {
                _ambientUnitOfWork.SetUnitOfWork(parentUow);
            };

            unitOfWork.Failed += (sender, args) =>
            {
                _ambientUnitOfWork.SetUnitOfWork(parentUow);
            };

            unitOfWork.Disposed += (sender, args) =>
            {
                scope.Dispose();
            };

            return _ambientUnitOfWork.UnitOfWork;
        }
    }
}