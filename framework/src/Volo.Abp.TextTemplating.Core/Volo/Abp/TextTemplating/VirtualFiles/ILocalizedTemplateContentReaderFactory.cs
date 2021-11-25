using System.Threading.Tasks;

namespace Volo.Abp.TextTemplating.VirtualFiles;

public interface ILocalizedTemplateContentReaderFactory
{
    Task<ILocalizedTemplateContentReader> CreateAsync(TemplateDefinition templateDefinition);
}
