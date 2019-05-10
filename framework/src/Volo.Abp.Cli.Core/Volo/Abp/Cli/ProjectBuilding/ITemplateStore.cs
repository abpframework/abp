using System.Threading.Tasks;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public interface ITemplateStore
    {
        Task<TemplateFile> GetAsync(
            string name,
            string version,
            DatabaseProvider databaseProvider,
            string projectName
        );
    }
}