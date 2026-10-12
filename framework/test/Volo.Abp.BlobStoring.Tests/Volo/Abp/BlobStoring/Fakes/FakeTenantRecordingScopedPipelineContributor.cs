using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// Holds a <see cref="FakeTenantRecordingScopedService"/> in the contributor scope
/// without transforming the content.
/// </summary>
public class FakeTenantRecordingScopedPipelineContributor : IBlobPipelineContributor, ITransientDependency
{
    // ReSharper disable once NotAccessedField.Local
    private readonly FakeTenantRecordingScopedService _service;

    public FakeTenantRecordingScopedPipelineContributor(FakeTenantRecordingScopedService service)
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
