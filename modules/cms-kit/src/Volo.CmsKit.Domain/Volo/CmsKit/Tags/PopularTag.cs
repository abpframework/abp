using System;

namespace Volo.CmsKit.Tags;

public class PopularTag
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Count { get; set; }
    
    public PopularTag(Guid id, string name, int count)
    {
        Id = id;
        Name = name;
        Count = count;
    }
}