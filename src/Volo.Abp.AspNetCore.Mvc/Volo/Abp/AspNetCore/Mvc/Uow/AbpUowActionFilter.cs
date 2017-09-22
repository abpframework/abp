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
            _unitOfWorkManager.BeginReserved(UnitOfWorkReservationName);
            await next();
        }
    }
}
