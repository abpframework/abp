using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// Holds an async-only disposable scoped service without transforming the content.
/// </summary>
public class FakeAsyncScopedPipelineContributor : IBlobPipelineContributor, ITransientDependency
{
    // ReSharper disable once NotAccessedField.Local
    private readonly FakeAsyncOnlyDisposableService _service;

    public FakeAsyncScopedPipelineContributor(FakeAsyncOnlyDisposableService service)
    {
        _service = service;
    }

    public Task OnSavingAsync(BlobPipelineContext context)
    {
        return Task.CompletedTask;
    }

    public Task OnGettingAsync(BlobPipelineContext context)
    {
        return Task.CompletedTask;
    }
}
