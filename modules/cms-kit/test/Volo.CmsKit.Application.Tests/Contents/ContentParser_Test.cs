using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Volo.CmsKit.Contents;
public class ContentParser_Test : CmsKitDomainTestBase
{
    private readonly CmsKitTestData testData;
    private readonly IOptions<CmsKitContentWidgetOptions> _options;
    private ContentParser contentParser;

    public ContentParser_Test()
    {
        testData = GetRequiredService<CmsKitTestData>();
        _options = GetRequiredService<IOptions<CmsKitContentWidgetOptions>>();
    }

    [Fact]
    public async Task ParseAsync_ShouldWorkWithDifferentWidgetTypes()
    {
        _options.Value.AddWidget(testData.PollName, testData.WidgetName, string.Empty);
        _options.Value.AddWidget("ImageGallery", "ImageGallery", string.Empty);
        contentParser = new ContentParser(_options);

        var content = @"**ABP Framework** is completely open source and developed in a community-driven manner.
                        [Widget Type=""Poll"" Code=""poll-name""]
                        Thanks _for_ *your * feedback.
                        [Widget GalleryName=""Xyz"" Type=""ImageGallery"" Source=""GoogleDrive""]";

        var widgets = await contentParser.ParseAsync(content);

        widgets.ShouldNotBeNull();
        widgets.Count.ShouldBe(4);
        widgets[1].ExtraProperties.Count.ShouldBe(2);
        widgets[3].ExtraProperties.Count.ShouldBe(3);
    }

    [Fact]
    public async Task ParseAsync_ShouldWorkWithoutConfigOptions()
    {
        var content = @"**ABP Framework** is completely open source and developed in a community-driven manner.
                        [Widget Type=  ""Poll"" Code =""poll-name""]
                        Thanks _for_ *your * feedback.";

        contentParser = new ContentParser(_options);
        var widgets = await contentParser.ParseAsync(content);

        widgets.ShouldNotBeNull();
        widgets.Count.ShouldBe(1);//Ignored Widget
    }

    [Fact]
    public async Task ParseAsync_ShouldWorkWithWrongConfigOptions()
    {
        _options.Value.AddWidget(testData.WidgetName, testData.PollName, string.Empty);
        contentParser = new ContentParser(_options);

        var content = @"**ABP Framework** is completely open source and developed in a community-driven manner.
                        [Widget Type=  ""Poll"" Code =""poll-name""]
                        Thanks _for_ *your * feedback.";

        var widgets = await contentParser.ParseAsync(content);

        widgets.ShouldNotBeNull();
        widgets.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ParseAsync_ShouldWorkWithWrongWidgetType()
    {
        _options.Value.AddWidget(testData.PollName, testData.WidgetName, string.Empty);
        contentParser = new ContentParser(_options);

        var content = @"**ABP Framework** is completely open source and developed in a community-driven manner.
                        [Widget Wrong Type=  ""Poll"" Code =""poll-name""]
                        Thanks _for_ *your * feedback.";

        var widgets = await contentParser.ParseAsync(content);

        widgets.ShouldNotBeNull();
        widgets.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ParseAsync_ShouldWorkWithWrongPollName()
    {
        _options.Value.AddWidget(testData.PollName, testData.WidgetName, string.Empty);
        contentParser = new ContentParser(_options);

        var content = @"**ABP Framework** is completely open source and developed in a community-driven manner.
                        [Widget Type=   ""Poll"" PollWrongName =""poll-name""]
                        Thanks _for_ *your * feedback.";

        var widgets = await contentParser.ParseAsync(content);

        widgets.ShouldNotBeNull();
        widgets.Count.ShouldBe(3);
    }

    [Theory]
    [MemberData(nameof(ExampleData))]
    public async Task ParseAsync_ShouldWorkProperlyWithCorrectInputs(string content, int expectedLine)
    {
        _options.Value.AddWidget(testData.PollName, testData.WidgetName, string.Empty);
        contentParser = new ContentParser(_options);

        var widgets = await contentParser.ParseAsync(content);

        widgets.ShouldNotBeNull();
        widgets.Count.ShouldBe(expectedLine);
    }

    public static IEnumerable<object[]> ExampleData =>
         new List<object[]>
         {
              new object[] { @"**ABP Framework** is completely open source and developed in a community-driven manner.
                        [Widget Type=""Poll"" Code=""poll-name""]
                        Thanks _for_ *your * feedback.", 3},

              new object[] { @"**ABP Framework** is completely open source and developed in a community-driven manner.
                    [Widget Type=""Poll"" Code=""poll-name""]
                    Thanks _for_ *your * feedback.
                    [Widget Type=""Poll"" Code=""poll-name1""]", 4 },

              new object[] { @"**ABP Framework** is completely open source and developed in a community-driven manner.
                    Thanks _for_ *your * feedback.
                    [Widget Type=""Poll"" Code=""poll-name""]", 2 },

              new object[] {   @"[Widget Type=""Poll"" Code=""poll-name""] gg [Widget Type=""Poll"" Code=""poll-name1""]**ABP Framework** is completely open source and developed in a community-driven manner.
                    Thanks _for_ *your * feedback.
                    Thanks _for_ *your * feedback.", 4},

              new object[] { @"Thanks _for_ *your * feedback.
                    Thanks _for_ *your * feedback.", 1}
         };
}
