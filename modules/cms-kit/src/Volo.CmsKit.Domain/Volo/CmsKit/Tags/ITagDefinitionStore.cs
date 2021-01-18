using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.CmsKit.Tags
{
    public interface ITagDefinitionStore
    {
        Task<List<TagEntityTypeDefiniton>> GetTagDefinitionsAsync();

        Task<TagEntityTypeDefiniton> GetTagDefinitionAsync([NotNull] string entityType);

        Task<bool> IsDefinedAsync([NotNull] string entityType);
    }
}
