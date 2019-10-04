using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public interface ITemplateStore
    {
        Task<TemplateFile> GetAsync(
            string name,
            DatabaseProvider databaseProvider,
            string projectName,
            [CanBeNull] string version = null
        );
    }
}