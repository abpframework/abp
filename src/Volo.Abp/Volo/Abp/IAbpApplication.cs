using System;
using Volo.Abp.Modularity;

namespace Volo.Abp
{
    public interface IAbpApplication
    {
        Type StartupModuleType { get; }

        IServiceProvider ServiceProvider { get; }

        AbpModuleDescriptor[] Modules { get; }

        void Shutdown();
    }
}