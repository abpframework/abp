using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Pages;

public interface IContentParser
{
    Task<List<ContentFragment>> ParseAsync(string content);
}