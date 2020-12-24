using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUnitOfWorkTransactionBehaviourProvider _transactionBehaviourProvider;
        private readonly AbpUnitOfWorkDefaultOptions _defaultOptions;

        public UnitOfWorkInterceptor(
            IUnitOfWorkManager unitOfWorkManager,
            IOptions<AbpUnitOfWorkDefaultOptions> options,
            IUnitOfWorkTransactionBehaviourProvider transactionBehaviourProvider)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _transactionBehaviourProvider = transactionBehaviourProvider;
            _defaultOptions = options.Value;
        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            if (!UnitOfWorkHelper.IsUnitOfWorkMethod(invocation.Method, out var unitOfWorkAttribute))
            {
                await invocation.ProceedAsync();
                return;
            }

            var options = CreateOptions(invocation, unitOfWorkAttribute);

            //Trying to begin a reserved UOW by AbpUnitOfWorkMiddleware
            if (_unitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, options))
            {
                await invocation.ProceedAsync();
                return;
            }

            using (var uow = _unitOfWorkManager.Begin(options))
            {
                await invocation.ProceedAsync();
                await uow.CompleteAsync();
            }
        }

        private AbpUnitOfWorkOptions CreateOptions(IAbpMethodInvocation invocation, [CanBeNull] UnitOfWorkAttribute unitOfWorkAttribute)
        {
            var options = new AbpUnitOfWorkOptions();

            unitOfWorkAttribute?.SetOptions(options);

            if (unitOfWorkAttribute?.IsTransactional == null)
            {
                options.IsTransactional = _defaultOptions.CalculateIsTransactional(
                    autoValue: _transactionBehaviourProvider.IsTransactional
                               ?? !invocation.Method.Name.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase)
                );
            }

            return options;
        }
    }
}
