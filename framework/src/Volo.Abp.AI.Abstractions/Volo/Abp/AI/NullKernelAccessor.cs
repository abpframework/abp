
using Microsoft.SemanticKernel;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AI;

[Dependency(TryRegister = true)]
[ExposeServices(typeof(IKernelAccessor))]
public class NullKernelAccessor : IKernelAccessor
{
    public Kernel? Kernel => null;
}

[Dependency(TryRegister = true)]
[ExposeServices(typeof(IKernelAccessor<>))]
public class NullKernelAccessor<TWorkSpace> : IKernelAccessor<TWorkSpace>
    where TWorkSpace : class
{
    public Kernel? Kernel => null;
}