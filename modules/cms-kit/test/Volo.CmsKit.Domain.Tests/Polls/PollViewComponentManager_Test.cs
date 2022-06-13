using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Volo.CmsKit.Polls;

public class PollViewComponentManager_Test : CmsKitDomainTestBase
{
    private readonly PollViewComponentManager pollManager;
    private readonly CmsKitTestData testData;
    private readonly IOptions<CmsKitContentWidgetOptions> _options;

    public PollViewComponentManager_Test()
    {
        pollManager = GetRequiredService<PollViewComponentManager>();
        testData = GetRequiredService<CmsKitTestData>();
        _options = GetRequiredService<IOptions<CmsKitContentWidgetOptions>>();
    }

    [Fact]
    public async Task AA_ParseAsync_ShouldWorkMoreDynamically()
    {
        _options.Value.AddWidgetConfig(testData.PollName, new ContentWidgetConfig(testData.WidgetName));
        _options.Value.AddWidgetConfig("ImageGallery", new ContentWidgetConfig("ImageGallery"));//test

        var content = @"**ABP Framework** is completely open source and developed in a community-driven manner.
                        [Widget Type=""Poll"" PollName =""poll-name""]
                        Thanks _for_ *your * feedback.
                        [Widget Type=""ImageGallery"" GalleryName=""Xyz"" Source=""GoogleDrive""]";

        var poll = await pollManager.ParseAsync(content);

        poll.ShouldNotBeNull();
        poll.Count.ShouldBe(4);
    }

    [Fact]
    public async Task ParseAsync_ShouldWorkWithoutConfigOptions()
    {
        var content = @"**ABP Framework** is completely open source and developed in a community-driven manner.
                        [Widget Type=  ""Poll"" PollName =""poll-name""]
                        Thanks _for_ *your * feedback.";

        var poll = await pollManager.ParseAsync(content);

        poll.ShouldNotBeNull();
        poll.Count.ShouldBe(1);//Ignored Widget
    }

    [Fact]
    public async Task ParseAsync_ShouldWorkWithWrongConfigOptions()
    {
        _options.Value.AddWidgetConfig(testData.WidgetName, new ContentWidgetConfig(testData.PollName));

        var content = @"**ABP Framework** is completely open source and developed in a community-driven manner.
                        [Widget Type=  ""Poll"" PollName =""poll-name""]
                        Thanks _for_ *your * feedback.";

        var poll = await pollManager.ParseAsync(content);

        poll.ShouldNotBeNull();
        poll.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ParseAsync_ShouldWorkWithWrongWidgetType()
    {
        _options.Value.AddWidgetConfig(testData.PollName, new ContentWidgetConfig(testData.WidgetName));

        var content = @"**ABP Framework** is completely open source and developed in a community-driven manner.
                        [Widget Wrong Type=  ""Poll"" PollName =""poll-name""]
                        Thanks _for_ *your * feedback.";

        var poll = await pollManager.ParseAsync(content);

        poll.ShouldNotBeNull();
        poll.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ParseAsync_ShouldWorkWithWrongPollName()
    {
        _options.Value.AddWidgetConfig(testData.PollName, new ContentWidgetConfig(testData.WidgetName));

        var content = @"**ABP Framework** is completely open source and developed in a community-driven manner.
                        [Widget Type=  ""Poll"" PollWrongName =""poll-name""]
                        Thanks _for_ *your * feedback.";

        var poll = await pollManager.ParseAsync(content);

        poll.ShouldNotBeNull();
        poll.Count.ShouldBe(2);
    }

    [Theory]
    [MemberData(nameof(ExampleData))]
    public async Task ParseAsync_ShouldWorkProperlyWithCorrectInputs(string content, int expectedLine)
    {
        _options.Value.AddWidgetConfig(testData.PollName, new ContentWidgetConfig(testData.WidgetName));

        var poll = await pollManager.ParseAsync(content);

        poll.ShouldNotBeNull();
        poll.Count.ShouldBe(expectedLine);
    }

    public static IEnumerable<object[]> ExampleData =>
         new List<object[]>
         {
              new object[] { @"**ABP Framework** is completely open source and developed in a community-driven manner.
                        [Widget Type=""Poll"" PollName=""poll-name""]
                        Thanks _for_ *your * feedback.", 3},

              new object[] { @"**ABP Framework** is completely open source and developed in a community-driven manner.
                    [Widget Type=""Poll"" PollName=""poll-name""]
                    Thanks _for_ *your * feedback.
                    [Widget Type=""Poll"" PollName=""poll-name1""]", 4 },

              new object[] { @"**ABP Framework** is completely open source and developed in a community-driven manner.
                    Thanks _for_ *your * feedback.
                    [Widget Type=""Poll"" PollName=""poll-name""]", 2 },

              new object[] {   @"[Widget Type=""Poll"" PollName=""poll-name""] gg [Widget Type=""Poll"" PollName=""poll-name1""]**ABP Framework** is completely open source and developed in a community-driven manner.
                    Thanks _for_ *your * feedback.
                    Thanks _for_ *your * feedback.", 4},

              new object[] { @"Thanks _for_ *your * feedback.
                    Thanks _for_ *your * feedback.", 1}
         };

}






