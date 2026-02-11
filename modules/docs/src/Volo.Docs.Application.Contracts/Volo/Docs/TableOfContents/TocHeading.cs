using System.Collections.Generic;

namespace Volo.Docs.TableOfContents;

public class TocHeading
{
    public int Level { get; set; }

    public string Text { get; set; }

    public string Id { get; set; }

    public TocHeading(int level, string text, string id)
    {
        Level = level;
        Text = text;
        Id = id;
    }
}

public class TocItem
{
    public TocHeading Heading { get; set; }

    public List<TocItem> Children { get; set; }

    public TocItem(TocHeading heading, List<TocItem> children)
    {
        Heading = heading;
        Children = children;
    }
}