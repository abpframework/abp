using System.Threading.Tasks;

namespace Volo.Abp.Gdpr;

public abstract class GdprUserDataProviderBase : IGdprUserDataProvider
{
    public abstract Task<GdprDataInfo> GetAsync(GdprUserDataProviderContext context);
}