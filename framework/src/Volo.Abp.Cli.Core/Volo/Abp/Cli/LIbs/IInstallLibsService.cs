using System.Threading.Tasks;

namespace Volo.Abp.Cli.LIbs
{
    public interface IInstallLibsService
    {
        Task InstallLibs(string directory);
    }
}
