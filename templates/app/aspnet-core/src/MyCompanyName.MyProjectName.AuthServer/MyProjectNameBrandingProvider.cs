using Microsoft.Extensions.Localization;
using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace MyCompanyName.MyProjectName;

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
