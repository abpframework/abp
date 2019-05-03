using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ProjectBuilding
{
    public class AbpIoTemplateStore : ITemplateStore, ITransientDependency
    {
        public async Task<TemplateFile> GetAsync(string templateName, string version)
        {
            return new TemplateFile(
                File.ReadAllBytes("C:\\Temp\\abp-templates\\" + version + "\\" + templateName + ".zip")
            );
        }
    }
}