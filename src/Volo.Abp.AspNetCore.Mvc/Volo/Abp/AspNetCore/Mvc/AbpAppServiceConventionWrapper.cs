using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc
{
    [DisableConventionalRegistration]
    public class AbpAppServiceConventionWrapper : IApplicationModelConvention
    {
        private readonly Lazy<IAbpAppServiceConvention> _convention;

        public AbpAppServiceConventionWrapper(IServiceCollection services)
        {
            _convention = services.GetRequiredServiceLazy<IAbpAppServiceConvention>();
        }

        public void Apply(ApplicationModel application)
        {
            _convention.Value.Apply(application);
        }
    }
}