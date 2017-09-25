using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkManager : IUnitOfWorkManager, ISingletonDependency
    {
        public IUnitOfWork Current => GetCurrentUnitOfWork();

        private readonly IServiceProvider _serviceProvider;
        private readonly IAmbientUnitOfWork _ambientUnitOfWork;

        public UnitOfWorkManager(IServiceProvider serviceProvider, IAmbientUnitOfWork ambientUnitOfWork)
        {
            _serviceProvider = serviceProvider;
            _ambientUnitOfWork = ambientUnitOfWork;
        }

        public IBasicUnitOfWork Begin(UnitOfWorkStartOptions options)
        {
            Check.NotNull(options, nameof(options));

            if (!options.RequiresNew && _ambientUnitOfWork.UnitOfWork != null && !_ambientUnitOfWork.UnitOfWork.IsReserved)
            {
                return new ChildUnitOfWork(_ambientUnitOfWork.UnitOfWork);
            }

            if (_ambientUnitOfWork.UnitOfWork != null)
            {
                //Requires new because there is already a current UOW but it's reserved
                options.RequiresNew = true;
            }

            return CreateUnitOfWork(options);
        }

        public void BeginReserved(string reservationName, UnitOfWorkStartOptions options)
        {
            if (!TryBeginReserved(reservationName, options))
            {
                throw new AbpException($"Could not find a reserved unit of work with reservation name: {reservationName}");
            }
        }

        public bool TryBeginReserved(string reservationName, UnitOfWorkStartOptions options)
        {
            Check.NotNull(reservationName, nameof(reservationName));

            var uow = _ambientUnitOfWork.UnitOfWork;

            //Find reserved unit of work starting from current and going to outers
            while (uow != null && !uow.IsReservedFor(reservationName))
            {
                uow = uow.Outer;
            }

            if (uow == null)
            {
                return false;
            }

            uow.IsReserved = false;
            uow.SetOptions(options);
            return true;
        }

        private IUnitOfWork GetCurrentUnitOfWork()
        {
            var uow = _ambientUnitOfWork.UnitOfWork;

            //Skip reserved unit of work
            while (uow != null && uow.IsReserved)
            {
                uow = uow.Outer;
            }

            return uow;
        }

        private IUnitOfWork CreateUnitOfWork(UnitOfWorkStartOptions options)
        {
            var scope = _serviceProvider.CreateScope();

            try
            {
                var outerUow = _ambientUnitOfWork.UnitOfWork;

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                unitOfWork.SetOuter(outerUow);
                unitOfWork.IsReserved = options.ReservationName != null;
                unitOfWork.ReservationName = options.ReservationName;
                unitOfWork.SetOptions(options); //TODO: Should not call this for reservation?

                _ambientUnitOfWork.SetUnitOfWork(unitOfWork);

                unitOfWork.Disposed += (sender, args) =>
                {
                    _ambientUnitOfWork.SetUnitOfWork(outerUow);
                    scope.Dispose();
                };

                return unitOfWork;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
        }
    }
}