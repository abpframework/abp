﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Uow
{
    public class AbpUnitOfWorkMiddleware : IMiddleware, ITransientDependency
    {
        public const string UnitOfWorkReservationName = "_AbpActionUnitOfWork";

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public AbpUnitOfWorkMiddleware(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using (var uow = _unitOfWorkManager.Reserve(UnitOfWorkReservationName))
            {
                await next(context);
                await uow.CompleteAsync(context.RequestAborted);
            }
        }
    }
}
