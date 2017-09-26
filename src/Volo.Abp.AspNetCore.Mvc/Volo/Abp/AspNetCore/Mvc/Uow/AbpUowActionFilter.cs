using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Mvc.Uow
{
    public class AbpUowActionFilter : IAsyncActionFilter, ITransientDependency
    {
        public const string UnitOfWorkReservationName = "_AbpActionUnitOfWork";

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly UnitOfWorkDefaultOptions _defaultOptions;

        public AbpUowActionFilter(IUnitOfWorkManager unitOfWorkManager, IOptions<UnitOfWorkDefaultOptions> options)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _defaultOptions = options.Value;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionDescriptor.IsControllerAction())
            {
                await next();
                return;
            }

            var methodInfo = context.ActionDescriptor.GetMethodInfo();
            var unitOfWorkAttr = UnitOfWorkHelper.GetUnitOfWorkAttributeOrNull(methodInfo);

            if (unitOfWorkAttr?.IsDisabled == true)
            {
                await next();
                return;
            }

            var options = CreateOptions(context, unitOfWorkAttr);

            if (_unitOfWorkManager.TryBeginReserved(UnitOfWorkReservationName, options))
            {
                await next();
                return;
            }

            using (var uow = _unitOfWorkManager.Begin(options))
            {
                var result = await next();
                if (result.Exception == null || result.ExceptionHandled)
                {
                    await uow.CompleteAsync(context.HttpContext.RequestAborted);
                }
            }
        }

        private UnitOfWorkOptions CreateOptions(ActionExecutingContext context, UnitOfWorkAttribute unitOfWorkAttr)
        {
            var options = new UnitOfWorkOptions();

            unitOfWorkAttr?.SetOptions(options);

            options.IsTransactional = _defaultOptions.CalculateIsTransactional(
                autoValue: !string.Equals(context.HttpContext.Request.Method, HttpMethod.Get.Method, StringComparison.OrdinalIgnoreCase)
            );

            return options;
        }
    }
}
