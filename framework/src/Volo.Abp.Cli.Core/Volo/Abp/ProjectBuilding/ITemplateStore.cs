using System.Threading.Tasks;

namespace Volo.Abp.ProjectBuilding
{
    public interface ITemplateStore
    {
        Task<TemplateFile> GetAsync(string templateInfoName, string version);
    }
}