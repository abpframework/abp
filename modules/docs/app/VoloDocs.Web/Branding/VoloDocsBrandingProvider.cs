using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Localization;

namespace VoloDocs.Web.Branding
{
    [Dependency(ReplaceServices = true)]
    public class VoloDocsBrandingProvider : DefaultBrandingProvider
    {
        public VoloDocsBrandingProvider(IConfiguration configuration, IStringLocalizer<DocsResource> localizer)
        {
            AppName = localizer["DocsTitle"];

            if (configuration["LogoUrl"] != null)
            {
                LogoUrl = configuration["LogoUrl"];
            }
        }

        public override string AppName { get; }

        public override string LogoUrl { get; }
    }
}
