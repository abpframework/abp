using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.CmsKit.Blogs
{
    public interface IBlogManager
    {
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}