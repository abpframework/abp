using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.RazorPages
{
    [Dependency(ReplaceServices = true)]
    public class ServiceBasedPageModelActivatorProvider : IPageModelActivatorProvider, ITransientDependency
    {
        public Func<PageContext, object> CreateActivator([NotNull] CompiledPageActionDescriptor descriptor)
        {
            Check.NotNull(descriptor, nameof(descriptor));
            return context => context.HttpContext.RequestServices.GetRequiredService(descriptor.ModelTypeInfo);
        }

        public Action<PageContext, object> CreateReleaser([NotNull] CompiledPageActionDescriptor descriptor)
        {
            return null;
        }
    }
}
