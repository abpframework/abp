using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.Configuration;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs.Branding
{
    [Dependency(ReplaceServices = true)]
    public class VoloDocsBrandingProvider : DefaultBrandingProvider
    {
        public VoloDocsBrandingProvider(IConfigurationAccessor configurationAccessor)
        {
            var configuration = configurationAccessor.Configuration;

            if (configuration["Title"] != null)
            {
                AppName = configuration["Title"];
            }

            if (configuration["LogoUrl"] != null)
            {
                LogoUrl = configuration["LogoUrl"];
            }
        }

        public override string AppName { get; }

        public override string LogoUrl { get; }
    }
}
