using System;
using System.Collections.Generic;

namespace Volo.Docs.Documents.Filter;

public class FilterVersionItem
{
    public string Version { get; set; }
    public IEnumerable<Guid> ProjectIds { get; set; }
    public IEnumerable<string> Languages { get; set; }
}