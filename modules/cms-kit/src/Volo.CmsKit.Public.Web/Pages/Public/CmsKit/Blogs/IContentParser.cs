using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.CmsKit.Public.Blogs;

namespace Volo.CmsKit.Public.Web.Pages.Public.CmsKit.Blogs;

public interface IContentParser
{
    Task<List<ContentFragment>> ParseAsync(string content);
}