using JetBrains.Annotations;
using System.Threading.Tasks;

namespace Volo.CmsKit.MediaDescriptors
{
    public interface IMediaDescriptorDefinitionStore
    {
        Task<bool> IsDefinedAsync([NotNull] string entityType);

        Task<MediaDescriptorDefinition> GetDefinitionAsync([NotNull] string entityType);
    }
}
