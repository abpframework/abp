#nullable enable
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// Sets the content stream back to a stream chosen by the test (the caller's
/// original), so tests can verify the pipeline never treats it as its own.
/// </summary>
public class FakeOriginalRestoringPipelineContributor : IBlobPipelineContributor, ITransientDependency
{
    public static Stream? RestoreTo { get; set; }

    public Task OnSavingAsync(BlobPipelineContext context)
    {
        if (RestoreTo != null)
        {
            context.BlobStream = RestoreTo;
        }

        return Task.CompletedTask;
    }

    public Task OnGettingAsync(BlobPipelineContext context)
    {
        return Task.CompletedTask;
    }
}
