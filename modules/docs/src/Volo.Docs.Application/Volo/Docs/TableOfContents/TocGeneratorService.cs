using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs.TableOfContents;

public class TocGeneratorService : ITocGeneratorService, ITransientDependency
{
    private const int MinHeadingLevel = 1;
    private const int MaxHeadingLevel = 6;

    public virtual List<TocHeading> GenerateTocHeadings(string markdownContent)
    {
        if (markdownContent.IsNullOrWhiteSpace())
        {
            return new List<TocHeading>();
        }

        var markdownPipeline = CreateMarkdownPipeline();
        var document = Markdig.Markdown.Parse(markdownContent, markdownPipeline);
        var headingBlocks = document.Descendants<HeadingBlock>();

        return headingBlocks
            .Select(CreateTocHeading)
            .ToList();
    }

    public virtual List<TocItem> GenerateTocItems(List<TocHeading> tocHeadings, int topLevel, int levelCount)
    {
        var maxLevel = GetMaxLevel(tocHeadings, topLevel, levelCount);

        var filteredHeadings = tocHeadings
            .Where(heading => heading.Level >= topLevel && heading.Level <= maxLevel)
            .ToList();

        return BuildHierarchicalStructure(filteredHeadings, topLevel);
    }

    public virtual int GetTopLevel(List<TocHeading> headings)
    {
        return Enumerable.Range(MinHeadingLevel, MaxHeadingLevel)
            .FirstOrDefault(level => headings.Count(h => h.Level == level) > 1, MinHeadingLevel);
    }

    public virtual List<TocItem> GenerateTocItems(string markdownContent, int levelCount, int? topLevel = null)
    {
        var headings = GenerateTocHeadings(markdownContent);
        if (headings.Count == 0)
        {
            return new List<TocItem>();
        }

        var resolvedTopLevel = topLevel ?? GetTopLevel(headings);
        return GenerateTocItems(headings, resolvedTopLevel, levelCount);
    }

    protected virtual MarkdownPipeline CreateMarkdownPipeline()
    {
        return new MarkdownPipelineBuilder()
            .UseAutoIdentifiers(AutoIdentifierOptions.GitHub)
            .UseAdvancedExtensions()
            .Build();
    }

    protected virtual TocHeading CreateTocHeading(HeadingBlock headingBlock)
    {
        var plainText = GetPlainText(headingBlock.Inline);
        var id = headingBlock.GetAttributes().Id;
        return new TocHeading(headingBlock.Level, plainText, id);
    }

    protected virtual List<TocItem> BuildHierarchicalStructure(List<TocHeading> headings, int topLevel)
    {
        var result = new List<TocItem>();

        for (var i = 0; i < headings.Count; i++)
        {
            var currentHeading = headings[i];

            if (currentHeading.Level != topLevel)
            {
                continue;
            }

            var children = GetDirectChildren(headings, i, currentHeading.Level);
            result.Add(new TocItem(currentHeading, children));
        }

        return result;
    }

    protected virtual List<TocItem> GetDirectChildren(List<TocHeading> allHeadings, int parentIndex, int parentLevel)
    {
        var targetChildLevel = parentLevel + 1;
        var children = new List<TocItem>();

        for (var i = parentIndex + 1; i < allHeadings.Count; i++)
        {
            var heading = allHeadings[i];

            // Stop if we encounter a heading at the same level or higher than parent
            if (heading.Level <= parentLevel)
            {
                break;
            }

            // Only process direct children (not grandchildren)
            if (heading.Level != targetChildLevel)
            {
                continue;
            }

            var grandChildren = GetDirectChildren(allHeadings, i, heading.Level);
            children.Add(new TocItem(heading, grandChildren));
        }

        return children;
    }

    protected virtual string GetPlainText(ContainerInline container)
    {
        if (container == null)
        {
            return string.Empty;
        }

        // Optimization for simple case with single literal inline
        if (HasExactCount(container, 1) && container.First() is LiteralInline singleLiteral)
        {
            return singleLiteral.Content.ToString();
        }

        return ProcessInlineContent(container);
    }

    protected virtual string ProcessInlineContent(ContainerInline container)
    {
        var textBuilder = new StringBuilder();
        var processingQueue = new Queue<Inline>();

        // Enqueue all initial inlines
        foreach (var inline in container)
        {
            processingQueue.Enqueue(inline);
        }

        // Process each inline in the queue
        while (processingQueue.Count > 0)
        {
            var currentInline = processingQueue.Dequeue();
            AppendInlineText(currentInline, textBuilder, processingQueue);
        }

        return textBuilder.ToString();
    }

    protected virtual void AppendInlineText(Inline inline, StringBuilder builder, Queue<Inline> processingQueue)
    {
        switch (inline)
        {
            case LiteralInline literal:
                builder.Append(literal.Content);
                break;

            case CodeInline code:
                builder.Append(code.Content);
                break;

            case ContainerInline containerInline:
                foreach (var childInline in containerInline)
                {
                    processingQueue.Enqueue(childInline);
                }
                break;
        }
    }

    protected virtual int GetMaxLevel(List<TocHeading> tocHeadings, int topLevel, int levelCount)
    {
        return tocHeadings.Where(h => h.Level >= topLevel)
            .Select(h => h.Level)
            .Distinct()
            .OrderBy(level => level)
            .Skip(levelCount - 1)
            .FirstOrDefault(topLevel);
    }

    protected virtual bool HasExactCount<T>(IEnumerable<T> enumerable, int count)
    {
        var itemCount = 0;
        foreach (var _ in enumerable)
        {
            itemCount++;
            if (itemCount > count)
            {
                return false;
            }
        }
        
        return itemCount == count;
    }
}
