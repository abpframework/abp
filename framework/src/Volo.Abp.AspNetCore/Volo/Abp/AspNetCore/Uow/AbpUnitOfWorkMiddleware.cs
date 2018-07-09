using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Uow
{
    public class AbpUnitOfWorkMiddleware
    {
        public const string UnitOfWorkReservationName = "_AbpActionUnitOfWork";

        private readonly RequestDelegate _next;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public AbpUnitOfWorkMiddleware(RequestDelegate next, IUnitOfWorkManager unitOfWorkManager)
        {
            _next = next;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (var uow = _unitOfWorkManager.Reserve(UnitOfWorkReservationName))
            {
                await _next(httpContext);
                await uow.CompleteAsync(httpContext.RequestAborted);
            }
        }
    }
}
