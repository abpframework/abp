using System;

namespace Volo.CmsKit.Tags;

public class PopularTagDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Count { get; set; }
}