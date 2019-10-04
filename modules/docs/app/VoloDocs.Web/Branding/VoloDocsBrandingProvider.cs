using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.Configuration;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Localization;

namespace VoloDocs.Web.Branding
{
    [Dependency(ReplaceServices = true)]
    public class VoloDocsBrandingProvider : DefaultBrandingProvider
    {
        public VoloDocsBrandingProvider(IConfigurationAccessor configurationAccessor, IStringLocalizer<DocsResource> localizer)
        {
            var configuration = configurationAccessor.Configuration;

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
