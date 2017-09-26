using Volo.Abp.AspNetCore.Mvc.Uow;

namespace Microsoft.AspNetCore.Builder
{
    public static class AbpAspNetCoreMvcApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AbpUnitOfWorkMiddleware>();
        }
    }
}
