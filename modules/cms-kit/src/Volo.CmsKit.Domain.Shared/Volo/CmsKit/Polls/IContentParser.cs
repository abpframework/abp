using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.CmsKit.Polls;

public interface IContentParser
{
    Task<List<ContentFragment>> ParseAsync(string content);
}