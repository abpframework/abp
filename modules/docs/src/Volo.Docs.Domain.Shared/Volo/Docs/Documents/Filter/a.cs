using System;
using System.Collections.Generic;

namespace Volo.Docs.Documents.Filter;

public class FilterItems
{
    public IEnumerable<FilterProjectItem> Projects { get; set; }
    public IEnumerable<FilterVersionItem> Versions { get; set; }
    public IEnumerable<FilterLanguageCodeItem> Languages { get; set; }
    public IEnumerable<string> Formats { get; set; }
}
public class FilterProjectItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
public class FilterVersionItem
{
    public string Version { get; set; }
    public IEnumerable<Guid> ProjectIds { get; set; }
    public IEnumerable<string> Languages { get; set; }
}
public class FilterLanguageCodeItem
{
    public string Code { get; set; }
    public IEnumerable<Guid> ProjectIds { get; set; }
    public IEnumerable<string> Versions { get; set; }
}