using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AspNetCore.Mvc.UI.Layout;

public interface IPageEmbeddingService
{
    /// <summary>
    /// Determines if the current request should render the page in embedding mode (without layout elements)
    /// </summary>
    /// <param name="httpContext">The current HTTP context</param>
    /// <returns>True if the page should be rendered without layout elements</returns>
    bool IsEmbeddingRequest(HttpContext httpContext);
} 