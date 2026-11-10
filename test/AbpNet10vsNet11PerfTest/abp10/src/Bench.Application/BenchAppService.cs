using Bench.Localization;
using Volo.Abp.Application.Services;

namespace Bench;

/* Inherit your application services from this class.
 */
public abstract class BenchAppService : ApplicationService
{
    protected BenchAppService()
    {
        LocalizationResource = typeof(BenchResource);
    }
}
