using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.Uow;

namespace Microsoft.AspNetCore.Builder
{
    public static class AbpAspNetCoreMvcApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder app)
        {
            return app
                .UseAbpExceptionHandling()
                .UseMiddleware<AbpUnitOfWorkMiddleware>();
        }

        public static IApplicationBuilder UseAbpExceptionHandling(this IApplicationBuilder app) //TODO: Should this go to
        {
            //Prevent multiple add
            if (app.Properties.ContainsKey("_AbpExceptionHandlingMiddleware_Added")) //TODO: Constant
            {
                return app;
            }

            app.Properties["_AbpExceptionHandlingMiddleware_Added"] = true;
            return app.UseMiddleware<AbpExceptionHandlingMiddleware>();
        }
    }
}
