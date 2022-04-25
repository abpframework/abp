using System.Threading.Tasks;

namespace Volo.Abp.Gdpr;

public interface IGdprUserDataProvider
{
    Task<GdprDataInfo> GetAsync(GdprUserDataProviderContext context);
}