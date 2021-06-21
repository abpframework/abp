using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace MyCompanyName.MyProjectName.Blazor
{
    [Dependency(ReplaceServices = true)]
    public class MyProjectNameBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "MyProjectName";
    }
}
