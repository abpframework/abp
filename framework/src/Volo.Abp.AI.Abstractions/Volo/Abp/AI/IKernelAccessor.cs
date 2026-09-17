
using Microsoft.SemanticKernel;

namespace Volo.Abp.AI;

public interface IKernelAccessor
{
    Kernel? Kernel { get; }
}

public interface IKernelAccessor<TWorkSpace> : IKernelAccessor
    where TWorkSpace : class
{
}
