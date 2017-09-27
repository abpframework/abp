using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Mvc.Uow
{
    public class AbpUnitOfWorkMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public AbpUnitOfWorkMiddleware(RequestDelegate next, IUnitOfWorkManager unitOfWorkManager)
        {
            _next = next;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (var uow = _unitOfWorkManager.Reserve(AbpUowActionFilter.UnitOfWorkReservationName))
            {
                await _next(httpContext);
                await uow.CompleteAsync(httpContext.RequestAborted);
            }
        }
    }
}
