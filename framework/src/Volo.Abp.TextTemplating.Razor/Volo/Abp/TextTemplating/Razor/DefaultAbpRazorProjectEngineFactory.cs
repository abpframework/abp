using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.TextTemplating.Razor;

public class DefaultAbpRazorProjectEngineFactory : IAbpRazorProjectEngineFactory, ITransientDependency
{
    public virtual async Task<RazorProjectEngine> CreateAsync(Action<RazorProjectEngineBuilder> configure = null)
    {
        return RazorProjectEngine.Create(await CreateRazorConfigurationAsync(), EmptyProjectFileSystem.Empty, configure);
    }

    protected virtual Task<RazorConfiguration> CreateRazorConfigurationAsync()
    {
        return Task.FromResult(RazorConfiguration.Default);
    }
}
