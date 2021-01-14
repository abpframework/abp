using System.Threading;
using System.Threading.Tasks;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Domain.Volo.CmsKit.Contents
{
    public interface IContentManager
    {
        Task<Content> InsertAsync(Content content, CancellationToken cancellationToken = default);
    }
}
