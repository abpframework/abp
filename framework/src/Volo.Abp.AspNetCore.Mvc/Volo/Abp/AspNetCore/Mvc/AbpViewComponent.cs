using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.AspNetCore.Mvc
{
    public abstract class AbpViewComponent : ViewComponent
    {
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        protected Type ObjectMapperContext { get; set; }

        protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetService<IObjectMapper>(provider =>
            ObjectMapperContext == null
                ? provider.GetRequiredService<IObjectMapper>()
                : (IObjectMapper) provider.GetRequiredService(typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext)));
    }
}
