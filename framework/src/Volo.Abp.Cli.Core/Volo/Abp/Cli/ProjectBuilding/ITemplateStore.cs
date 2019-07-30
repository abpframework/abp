using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public interface ITemplateStore
    {
        Task<TemplateFile> GetAsync(
            string name,
            [CanBeNull] string version = null
        );
    }
}