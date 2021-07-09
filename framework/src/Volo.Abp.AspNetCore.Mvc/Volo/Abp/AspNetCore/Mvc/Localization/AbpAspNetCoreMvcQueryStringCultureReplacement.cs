using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Localization
{
    public class AbpAspNetCoreMvcQueryStringCultureReplacement : IQueryStringCultureReplacement, ITransientDependency
    {
        protected AbpQueryStringCultureReplacementOptions Options { get; }
        protected IServiceProvider ServiceProvider { get; }

        public AbpAspNetCoreMvcQueryStringCultureReplacement(
            IOptions<AbpQueryStringCultureReplacementOptions> queryStringCultureReplacementOptions,
            IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Options = queryStringCultureReplacementOptions.Value;
        }

        public virtual async Task ReplaceAsync(QueryStringCultureReplacementContext context)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                foreach (var provider in Options.QueryStringCultureReplacementProviders)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    await (scope.ServiceProvider.GetRequiredService(provider) as IQueryStringCultureReplacementProvider)
                        .ReplaceAsync(context);
                }
            }
        }
    }
}
