using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

namespace Volo.Abp.AI;

public class KernelAccessor<TWorkSpace> : IKernelAccessor<TWorkSpace>
    where TWorkSpace : class
{
    public Kernel? Kernel { get; }

    public KernelAccessor(IServiceProvider serviceProvider)
    {
        Kernel = serviceProvider.GetKeyedService<Kernel>(
            AbpAIWorkspaceOptions.GetKernelServiceKeyName(
                WorkspaceNameAttribute.GetWorkspaceName<TWorkSpace>()));
    }
}


