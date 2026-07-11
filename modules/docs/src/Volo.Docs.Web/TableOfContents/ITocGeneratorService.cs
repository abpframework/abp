using System.Collections.Generic;

namespace Volo.Docs.TableOfContents;

public interface ITocGeneratorService
{
    List<TocHeading> GenerateTocHeadings(string markdownContent);
    
    List<TocItem> GenerateTocItems(List<TocHeading> tocHeadings, int topLevel, int levelCount);
    
    int GetTopLevel(List<TocHeading> tocHeadings);
    
    List<TocItem> GenerateTocItems(string markdownContent, int levelCount, int? topLevel = null);
}
