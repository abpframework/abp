using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Uow
{
    public class AbpUnitOfWorkMiddleware : IMiddleware, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public AbpUnitOfWorkMiddleware(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //TODO: Make this configurable.
            if (context.Request.Path.Value != null &&
                context.Request.Path.Value.StartsWith("/_blazor"))
            {
                await next(context);
                return;
            }
            
            using (var uow = _unitOfWorkManager.Reserve(UnitOfWork.UnitOfWorkReservationName))
            {
                await next(context);
                await uow.CompleteAsync(context.RequestAborted);
            }
        }
    }
}
