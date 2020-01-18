﻿using System;
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
        private readonly AbpUnitOfWorkDefaultOptions _defaultOptions;

        public UnitOfWorkInterceptor(IUnitOfWorkManager unitOfWorkManager, IOptions<AbpUnitOfWorkDefaultOptions> options)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _defaultOptions = options.Value;
        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            if (!UnitOfWorkHelper.IsUnitOfWorkMethod(invocation.Method, out var unitOfWorkAttribute))
            {
                await invocation.ProceedAsync().ConfigureAwait(false);
                return;
            }

            using (var uow = _unitOfWorkManager.Begin(CreateOptions(invocation, unitOfWorkAttribute)))
            {
                await invocation.ProceedAsync().ConfigureAwait(false);
                await uow.CompleteAsync().ConfigureAwait(false);
            }
        }

        private AbpUnitOfWorkOptions CreateOptions(IAbpMethodInvocation invocation, [CanBeNull] UnitOfWorkAttribute unitOfWorkAttribute)
        {
            var options = new AbpUnitOfWorkOptions();

            unitOfWorkAttribute?.SetOptions(options);

            if (unitOfWorkAttribute?.IsTransactional == null)
            {
                options.IsTransactional = _defaultOptions.CalculateIsTransactional(
                    autoValue: !invocation.Method.Name.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase)
                );
            }

            return options;
        }
    }
}
