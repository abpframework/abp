using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Mvc.Uow
{
    public class AbpUowActionFilter : IAsyncActionFilter, ITransientDependency
    {
        public const string UnitOfWorkReservationName = "_AbpActionUnitOfWork";

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public AbpUowActionFilter(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionDescriptor.IsControllerAction())
            {
                await next();
                return;
            }

            if (_unitOfWorkManager.TryBeginReserved(UnitOfWorkReservationName))
            {
                await next();
                return;
            }

            //TODO: Check if disabled. Get and apply attributes to control UOW.

            using (var uow = _unitOfWorkManager.Begin())
            {
                var result = await next();
                if (result.Exception == null || result.ExceptionHandled)
                {
                    await uow.CompleteAsync(context.HttpContext.RequestAborted);
                }
            }
        }
    }
}
