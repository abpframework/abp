using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Mvc.Uow
{
    public class AbpUnitOfWorkMiddleware
    {
        private readonly RequestDelegate _next;

        public AbpUnitOfWorkMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IUnitOfWorkManager unitOfWorkManager)
        {
            using (var uow = unitOfWorkManager.Reserve(AbpUowActionFilter.UnitOfWorkReservationName))
            {
                await _next(httpContext);
                await uow.CompleteAsync(httpContext.RequestAborted);
            }
        }
    }
}
