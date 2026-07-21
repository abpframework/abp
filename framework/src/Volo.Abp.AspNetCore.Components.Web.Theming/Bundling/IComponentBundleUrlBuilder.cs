using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Bundling;

public interface IComponentBundleUrlBuilder
{
    Task<string> BuildAsync(string fileName, string? appBasePath, string? navigationBaseUri);
}
