using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

public class FakeFailingGetPipelineContributor : IBlobPipelineContributor, ITransientDependency
{
    public Task OnSavingAsync(BlobPipelineContext context)
    {
        return Task.CompletedTask;
    }

    public Task OnGettingAsync(BlobPipelineContext context)
    {
        throw new InvalidOperationException("This contributor can not read content back!");
    }
}
