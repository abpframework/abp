using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// Wraps the content with a stream that only supports the modern async read
/// overload, so tests can verify the pipeline does not degrade such a stream
/// to the synchronous byte[] fallback of the Stream base class.
/// </summary>
public class FakeModernAsyncPipelineContributor : IBlobPipelineContributor, ITransientDependency
{
    public Task OnSavingAsync(BlobPipelineContext context)
    {
        return Task.CompletedTask;
    }

    public Task OnGettingAsync(BlobPipelineContext context)
    {
        context.BlobStream = new FakeModernAsyncOnlyStream(context.BlobStream);
        return Task.CompletedTask;
    }
}
