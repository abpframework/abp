using System.Threading.Tasks;

namespace Volo.Abp.IdentityServer.IdentityResources;

public interface IIdentityResourceDataSeeder
{
    Task CreateStandardResourcesAsync();
}
