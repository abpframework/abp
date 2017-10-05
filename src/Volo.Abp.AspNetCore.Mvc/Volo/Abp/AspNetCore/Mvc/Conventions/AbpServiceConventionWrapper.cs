using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Conventions
{
    [DisableConventionalRegistration]
    public class AbpServiceConventionWrapper : IApplicationModelConvention
    {
        private readonly Lazy<IAbpServiceConvention> _convention;

        public AbpServiceConventionWrapper(IServiceCollection services)
        {
            _convention = services.GetRequiredServiceLazy<IAbpServiceConvention>();
        }

        public void Apply(ApplicationModel application)
        {
            _convention.Value.Apply(application);
        }
    }
}