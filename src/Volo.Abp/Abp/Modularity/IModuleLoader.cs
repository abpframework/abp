using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public interface IModuleLoader : ISingletonDependency
    {
        IReadOnlyList<AbpModuleDescriptor> Modules { get; }

        void LoadAll(IServiceCollection services, Type startupModuleType);
    }
}
