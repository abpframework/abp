using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Uow;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Mvc.Uow
{
    public class AbpUowPageFilter : IAsyncPageFilter, ITransientDependency
    {
        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (context.HandlerMethod == null || !context.ActionDescriptor.IsPageAction())
            {
                await next();
                return;
            }

            var methodInfo = context.HandlerMethod.MethodInfo;
            var unitOfWorkAttr = UnitOfWorkHelper.GetUnitOfWorkAttributeOrNull(methodInfo);

            context.HttpContext.Items["_AbpActionInfo"] = new AbpActionInfoInHttpContext
            {
                IsObjectResult = ActionResultHelper.IsObjectResult(context.HandlerMethod.MethodInfo.ReturnType, typeof(void))
            };

            if (unitOfWorkAttr?.IsDisabled == true)
            {
                await next();
                return;
            }

            var options = CreateOptions(context, unitOfWorkAttr);

            var unitOfWorkManager = context.GetRequiredService<IUnitOfWorkManager>();

            //Trying to begin a reserved UOW by AbpUnitOfWorkMiddleware
            if (unitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, options))
            {
                var result = await next();
                if (Succeed(result))
                {
                    await SaveChangesAsync(context, unitOfWorkManager);
                }
                else
                {
                    await RollbackAsync(context, unitOfWorkManager);
                }

                return;
            }

            using (var uow = unitOfWorkManager.Begin(options))
            {
                var result = await next();
                if (Succeed(result))
                {
                    await uow.CompleteAsync(context.HttpContext.RequestAborted);
                }
                else
                {
                    await uow.RollbackAsync(context.HttpContext.RequestAborted);
                }
            }
        }

        private AbpUnitOfWorkOptions CreateOptions(PageHandlerExecutingContext context, UnitOfWorkAttribute unitOfWorkAttribute)
        {
            var options = new AbpUnitOfWorkOptions();

            unitOfWorkAttribute?.SetOptions(options);

            if (unitOfWorkAttribute?.IsTransactional == null)
            {
                var abpUnitOfWorkDefaultOptions = context.GetRequiredService<IOptions<AbpUnitOfWorkDefaultOptions>>().Value;
                options.IsTransactional = abpUnitOfWorkDefaultOptions.CalculateIsTransactional(
                    autoValue: !string.Equals(context.HttpContext.Request.Method, HttpMethod.Get.Method, StringComparison.OrdinalIgnoreCase)
                );
            }

            return options;
        }

        private async Task RollbackAsync(PageHandlerExecutingContext context, IUnitOfWorkManager unitOfWorkManager)
        {
            var currentUow = unitOfWorkManager.Current;
            if (currentUow != null)
            {
                await currentUow.RollbackAsync(context.HttpContext.RequestAborted);
            }
        }
        
        private async Task SaveChangesAsync(PageHandlerExecutingContext context, IUnitOfWorkManager unitOfWorkManager)
        {
            var currentUow = unitOfWorkManager.Current;
            if (currentUow != null)
            {
                await currentUow.SaveChangesAsync(context.HttpContext.RequestAborted);
            }
        }

        private static bool Succeed(PageHandlerExecutedContext result)
        {
            return result.Exception == null || result.ExceptionHandled;
        }
    }
}
