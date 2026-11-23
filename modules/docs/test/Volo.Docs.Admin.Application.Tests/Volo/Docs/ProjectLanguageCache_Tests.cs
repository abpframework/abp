using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Volo.Docs.Admin.Documents;
using Volo.Docs.Common.Projects;
using Volo.Docs.GitHub.Documents;
using Volo.Docs.GitHub.Documents.Version;
using Volo.Docs.Projects;
using Xunit;

namespace Volo.Docs;

public class ProjectLanguageCache_Tests : DocsAdminApplicationTestBase
{
    [Fact]
    public async Task ClearCache_Should_Invalidate_All_Version_Language_Entries()
    {
        var github = GetRequiredService<IGithubRepositoryManager>();
        github.GetVersionsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<GithubVersionProviderSource>())
            .Returns(new List<GithubVersion> { new() { Name = "2.0.0" }, new() { Name = "1.0.0" } });
        var language = "tr";
        var downloads = 0;
        github.GetFileRawStringContentAsync(Arg.Is<string>(x => x.EndsWith("docs-langs.json")), Arg.Any<string>(), Arg.Any<string>())
            .Returns(_ =>
            {
                downloads++;
                return "{\"languages\":[{\"code\":\"en\",\"isDefault\":true},{\"code\":\"" + language + "\",\"isDefault\":false}]}";
            });
        var projects = GetRequiredService<IProjectAppService>();
        (await projects.GetLanguageListAsync("abp", "1.0.0")).Languages.ShouldContain(x => x.Code == "tr");
        (await projects.GetLanguageListAsync("abp", "2.0.0")).Languages.ShouldContain(x => x.Code == "tr");
        downloads.ShouldBe(2);
        language = "de";
        (await projects.GetLanguageListAsync("abp", "1.0.0")).Languages.ShouldContain(x => x.Code == "tr");
        downloads.ShouldBe(2);

        await GetRequiredService<IDocumentAdminAppService>().ClearCacheAsync(new ClearCacheInput
        {
            ProjectId = GetRequiredService<DocsTestData>().ProjectId
        });

        (await projects.GetLanguageListAsync("abp", "1.0.0")).Languages.ShouldContain(x => x.Code == "de");
        (await projects.GetLanguageListAsync("abp", "2.0.0")).Languages.ShouldContain(x => x.Code == "de");
        downloads.ShouldBe(4);
    }
}
