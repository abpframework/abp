using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionPreConfigureExtensions
    {
        public static IServiceCollection PreConfigure<TOptions>(this IServiceCollection services, Action<TOptions> optionsAction)
        {
            services.GetPreConfigureActions<TOptions>().Add(optionsAction);
            return services;
        }

        public static PreConfigureActionList<TOptions> GetPreConfigureActions<TOptions>(this IServiceCollection services)
        {
            var actionList = services.GetSingletonInstanceOrNull<IObjectAccessor<PreConfigureActionList<TOptions>>>()?.Value;
            if (actionList == null)
            {
                actionList = new PreConfigureActionList<TOptions>();
                services.AddObjectAccessor(actionList);
            }

            return actionList;
        }
    }
}
