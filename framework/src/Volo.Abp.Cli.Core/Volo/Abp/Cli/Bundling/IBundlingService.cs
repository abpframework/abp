using System.Threading.Tasks;

namespace Volo.Abp.Cli.Bundling
{
    public interface IBundlingService
    {
        Task BundleAsync(string directory, bool forceBuild);
    }
}
