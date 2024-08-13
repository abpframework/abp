using Microsoft.Extensions.Localization;
using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace MyCompanyName.MyProjectName.Blazor.WebApp.Tiered;

[Dependency(ReplaceServices = true)]
public class MyProjectNameBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<MyProjectNameResource> _localizer;

    public MyProjectNameBrandingProvider(IStringLocalizer<MyProjectNameResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
