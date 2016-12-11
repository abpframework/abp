using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity.PlugIns;

namespace Volo.Abp.Modularity
{
    public interface IModuleLoader
    {
        IReadOnlyList<AbpModuleDescriptor> Modules { get; }

        void LoadAll(IServiceCollection services, Type startupModuleType, PlugInSourceList plugInSources);
    }
}
