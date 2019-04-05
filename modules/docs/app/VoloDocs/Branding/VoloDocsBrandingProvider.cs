using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;
namespace VoloDocs.Branding
{
    public class VoloDocsBrandingProvider
    {
        [Dependency(ReplaceServices = true)]
        public class MyProjectNameBrandingProvider : DefaultBrandingProvider
        {
            public override string AppName => "VoloDocs";
        }
    }
}
