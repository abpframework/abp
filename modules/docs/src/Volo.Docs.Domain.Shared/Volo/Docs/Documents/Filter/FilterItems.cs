using System.Collections.Generic;

namespace Volo.Docs.Documents.Filter;

public class FilterItems
{
    public IEnumerable<FilterProjectItem> Projects { get; set; }
    public IEnumerable<FilterVersionItem> Versions { get; set; }
    public IEnumerable<FilterLanguageCodeItem> Languages { get; set; }
    public IEnumerable<string> Formats { get; set; }
}