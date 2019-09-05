using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Microsoft.AspNetCore.RequestLocalization
{
    public class AbpRequestLocalizationMiddleware : IMiddleware, ITransientDependency
    {
        private readonly IAbpRequestLocalizationOptionsProvider _requestLocalizationOptionsProvider;

        public AbpRequestLocalizationMiddleware(
            IAbpRequestLocalizationOptionsProvider requestLocalizationOptionsProvider)
        {
            _requestLocalizationOptionsProvider = requestLocalizationOptionsProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var options = Options.Create(_requestLocalizationOptionsProvider.GetLocalizationOptions());
            var middleware = new RequestLocalizationMiddleware(next, options);
            await middleware.Invoke(context);
        }
    }
}