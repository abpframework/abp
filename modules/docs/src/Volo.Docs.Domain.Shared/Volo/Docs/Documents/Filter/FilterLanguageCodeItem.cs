using System;
using System.Collections.Generic;

namespace Volo.Docs.Documents.Filter;

public class FilterLanguageCodeItem
{
    public string Code { get; set; }
    public IEnumerable<Guid> ProjectIds { get; set; }
    public IEnumerable<string> Versions { get; set; }
}