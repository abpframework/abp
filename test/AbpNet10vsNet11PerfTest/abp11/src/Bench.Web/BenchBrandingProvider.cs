using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using Bench.Localization;

namespace Bench.Web;

[Dependency(ReplaceServices = true)]
public class BenchBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<BenchResource> _localizer;

    public BenchBrandingProvider(IStringLocalizer<BenchResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];

    public override string? LogoUrl => "~/images/logo/leptonxlite/logo-light-thumbnail.png";

    public override string? LogoIconUrl => "~/images/logo/leptonxlite/logo-dark-thumbnail.png";
}
