using System;
using System.Threading.Tasks;

namespace Volo.CmsKit.Blogs
{
    public interface IBlogManager
    {
        Task CheckDeleteAsync(Guid id);
    }
}