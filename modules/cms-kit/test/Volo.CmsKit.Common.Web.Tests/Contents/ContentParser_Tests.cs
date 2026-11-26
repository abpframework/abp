using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Data;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Web.Contents;
using Xunit;

namespace Volo.CmsKit.Web.Contents;

public class ContentParser_Tests
{
    private readonly ContentParser _parser;

    public ContentParser_Tests()
    {
        var options = new CmsKitContentWidgetOptions();
        options.AddWidget("Comment", "CmsCommenting");
        _parser = new ContentParser(Options.Create(options));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("# A document\n\nSome **Markdown**.")]
    public async Task Should_Preserve_Markdown(string content)
    {
        var fragments = await _parser.ParseAsync(content);

        fragments.Count.ShouldBe(1);
        fragments[0].Type.ShouldBe(ContentConsts.Markdown);
        fragments[0].GetProperty<string>(ContentConsts.Content).ShouldBe(content ?? string.Empty);
    }

    [Fact]
    public async Task Should_Parse_Widget_Parameters_With_Spaces()
    {
        var fragments = await _parser.ParseAsync("before\n[Widget Type=\"Comment\" Title=\"Hello world\" EntityId=\"42\"]\nafter");

        fragments.Count.ShouldBe(3);
        fragments[0].GetProperty<string>(ContentConsts.Content).ShouldBe("before\n");
        fragments[1].Type.ShouldBe(ContentConsts.Widget);
        fragments[1].GetProperty<string>("Type").ShouldBe("CmsCommenting");
        fragments[1].GetProperty<string>("Title").ShouldBe("Hello world");
        fragments[1].GetProperty<string>("EntityId").ShouldBe("42");
        fragments[2].GetProperty<string>(ContentConsts.Content).ShouldBe("\nafter");
    }

    [Fact]
    public async Task Should_Not_Interpret_Widgets_When_None_Are_Registered()
    {
        var parser = new ContentParser(Options.Create(new CmsKitContentWidgetOptions()));
        const string content = "[Widget Type=\"Comment\"]";

        var fragments = await parser.ParseAsync(content);

        fragments.Single().Type.ShouldBe(ContentConsts.Markdown);
        fragments.Single().GetProperty<string>(ContentConsts.Content).ShouldBe(content);
    }

    [Fact]
    public async Task Should_Keep_Opening_Brackets_In_Quoted_Values()
    {
        var fragments = await _parser.ParseAsync("[Widget Type=\"Comment\" Title=\"a[b\"] after");

        fragments.Count.ShouldBe(2);
        fragments[0].GetProperty<string>("Type").ShouldBe("CmsCommenting");
        fragments[0].GetProperty<string>("Title").ShouldBe("a[b");
        fragments[1].GetProperty<string>(ContentConsts.Content).ShouldBe(" after");
    }

    [Fact]
    public async Task Should_Parse_Multiple_Widgets_On_The_Same_Line()
    {
        var fragments = await _parser.ParseAsync("[Widget Type=\"Comment\" EntityId=\"1\"] between [Widget Type=\"Comment\" EntityId=\"2\"]");

        fragments.Count.ShouldBe(3);
        fragments[0].GetProperty<string>("EntityId").ShouldBe("1");
        fragments[1].GetProperty<string>(ContentConsts.Content).ShouldBe(" between ");
        fragments[2].GetProperty<string>("EntityId").ShouldBe("2");
    }

    [Fact]
    public async Task Should_Keep_Malformed_Markers_From_Consuming_Valid_Widgets()
    {
        var fragments = await _parser.ParseAsync("[Widget missing\n[Widget Type=\"Comment\" EntityId=\"42\"]");

        fragments.Count.ShouldBe(2);
        fragments[0].GetProperty<string>(ContentConsts.Content).ShouldBe("[Widget missing\n");
        fragments[1].GetProperty<string>("EntityId").ShouldBe("42");
    }

    [Fact]
    public async Task Should_Ignore_Unregistered_Widget_Types()
    {
        var fragments = await _parser.ParseAsync("[Widget Type=\"Unknown\"]\n[Widget Type=\"Comment\"]");

        fragments.Count(x => x.Type == ContentConsts.Widget).ShouldBe(1);
        fragments.Single(x => x.Type == ContentConsts.Widget).GetProperty<string>("Type").ShouldBe("CmsCommenting");
    }

    [Fact]
    public async Task Should_Handle_Repeated_Unterminated_Markers()
    {
        var content = string.Concat(Enumerable.Repeat("[Widget", 12000));

        var fragments = await ParseQuicklyAsync(content);

        fragments.Single().GetProperty<string>(ContentConsts.Content).ShouldBe(content);
    }

    [Fact]
    public async Task Should_Handle_Long_Malformed_Widget_Attributes()
    {
        var content = "[Widget Type=\"Comment\" " + new string(' ', 80000) + "Unterminated=\"value]";

        var fragments = await ParseQuicklyAsync(content);

        fragments.Single().Type.ShouldBe(ContentConsts.Widget);
        fragments.Single().GetProperty<string>("Type").ShouldBe("CmsCommenting");
        fragments.Single().HasProperty("Unterminated").ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Handle_Many_Consecutive_Widgets()
    {
        var content = string.Concat(Enumerable.Repeat("[Widget Type=\"Comment\"]\n", 10000));

        var fragments = await ParseQuicklyAsync(content);

        fragments.Count(x => x.Type == ContentConsts.Widget).ShouldBe(10000);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Should_Parse_Backtracking_Inputs_In_Linear_Time(int input)
    {
        var content = input switch
        {
            0 => string.Concat(Enumerable.Repeat("[Widget] ", 20000)),
            1 => "    " + string.Concat(Enumerable.Repeat("[Widget ", 10000)),
            _ => "[Widget Type=\"Comment\"" + new string(' ', 40000) + "]"
        };

        (await ParseQuicklyAsync(content)).ShouldNotBeEmpty();
    }

    // Parsing completes synchronously, so the elapsed time is asserted instead of relying on a test timeout.
    private async Task<List<ContentFragment>> ParseQuicklyAsync(string content)
    {
        var stopwatch = Stopwatch.StartNew();
        var fragments = await _parser.ParseAsync(content);
        stopwatch.Elapsed.ShouldBeLessThan(TimeSpan.FromSeconds(2));
        return fragments;
    }
}
