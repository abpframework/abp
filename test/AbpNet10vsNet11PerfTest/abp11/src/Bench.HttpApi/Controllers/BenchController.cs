using Bench.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Bench.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class BenchController : AbpControllerBase
{
    protected BenchController()
    {
        LocalizationResource = typeof(BenchResource);
    }
}
