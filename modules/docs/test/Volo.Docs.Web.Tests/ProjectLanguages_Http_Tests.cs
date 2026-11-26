using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Docs.Documents;
using Volo.Abp.Data;
using Volo.Docs.Projects;
using Volo.Abp.Caching;
using Volo.Docs.Caching;
using Xunit;

namespace Volo.Docs;

public class ProjectLanguages_Http_Tests : AbpWebApplicationFactoryIntegratedTest<DocsWebTestProgram>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseContentRoot(AppContext.BaseDirectory);
    }

    [Fact]
    public async Task Should_Not_Let_Unknown_Version_Poison_Real_Version_Languages()
    {
        var attack = await Client.GetAsync("/api/docs/projects/abp/no-such-version/languageList");
        attack.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var languages = await Client.GetFromJsonAsync<LanguageConfig>("/api/docs/projects/abp/dev/languageList");
        languages!.Languages.ShouldContain(x => x.Code == "tr");
    }

    [Fact]
    public async Task Should_Isolate_Two_Valid_Versions()
    {
        var older = await Client.GetFromJsonAsync<LanguageConfig>("/api/docs/projects/abp/1.0.0/languageList");
        var newer = await Client.GetFromJsonAsync<LanguageConfig>("/api/docs/projects/abp/2.0.0/languageList");
        older!.Languages.Count.ShouldBe(1);
        newer!.Languages.ShouldContain(x => x.Code == "tr");
    }

    [Fact]
    public async Task Should_Cache_Fallback_For_A_Valid_Version_Only_Briefly()
    {
        var state = ServiceProvider.GetRequiredService<DocsGithubTestState>();
        var cache = ServiceProvider.GetRequiredService<RecordingDistributedCache>();
        state.FailLanguageDownloads = true;
        var fallback = await Client.GetFromJsonAsync<LanguageConfig>("/api/docs/projects/abp/2.0.0/languageList");
        fallback!.Languages.Count.ShouldBe(1);
        (await Client.GetFromJsonAsync<LanguageConfig>("/api/docs/projects/abp/2.0.0/languageList"))!.Languages.Count.ShouldBe(1);
        state.LanguageDownloads.ShouldBe(1);

        var key = cache.Writes.Single(x => x.Contains(nameof(LanguageConfig)));
        cache.WriteOptions[key].AbsoluteExpirationRelativeToNow.ShouldBe(TimeSpan.FromMinutes(1));
        state.FailLanguageDownloads = false;
        await cache.RemoveAsync(key);

        var recovered = await Client.GetFromJsonAsync<LanguageConfig>("/api/docs/projects/abp/2.0.0/languageList");

        recovered!.Languages.ShouldContain(x => x.Code == "tr");
        state.LanguageDownloads.ShouldBe(2);
    }

    [Fact]
    public async Task Should_Cache_Languages_Of_A_Project_Without_Versions()
    {
        var projects = ServiceProvider.GetRequiredService<IProjectRepository>();
        var project = await projects.GetByShortNameAsync("product");
        project.LatestVersionBranchName = null;
        await projects.UpdateAsync(project, true);
        var cache = ServiceProvider.GetRequiredService<RecordingDistributedCache>();

        (await Client.GetFromJsonAsync<LanguageConfig>("/api/docs/projects/product/latest/languageList"))!.Languages.ShouldNotBeEmpty();
        (await Client.GetFromJsonAsync<LanguageConfig>("/api/docs/projects/product/latest/languageList"))!.Languages.ShouldNotBeEmpty();

        cache.Writes.Count(x => x.Contains(nameof(LanguageConfig))).ShouldBe(1);
    }
    [Fact]
    public async Task Should_Cache_Successful_Known_Version_Languages()
    {
        var first = await Client.GetFromJsonAsync<LanguageConfig>("/api/docs/projects/abp/2.0.0/languageList");
        var second = await Client.GetFromJsonAsync<LanguageConfig>("/api/docs/projects/abp/2.0.0/languageList");

        first!.Languages.Count.ShouldBe(2);
        second!.Languages.Count.ShouldBe(2);
        ServiceProvider.GetRequiredService<DocsGithubTestState>().LanguageDownloads.ShouldBe(1);
        ServiceProvider.GetRequiredService<RecordingDistributedCache>().Writes.ShouldContain(x => x.Contains(nameof(LanguageConfig)));
    }

    [Fact]
    public async Task Should_Not_Create_Language_Entries_For_Arbitrary_Unlisted_Labels()
    {
        for (var i = 0; i < 20; i++)
        {
            var response = await Client.GetAsync($"/api/docs/projects/product/custom-{i}/languageList");
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        ServiceProvider.GetRequiredService<RecordingDistributedCache>().Writes.ShouldNotContain(x => x.Contains(nameof(LanguageConfig)));
    }

    [Fact]
    public async Task Should_Not_Create_Language_Entries_For_Unknown_Listed_Source_Versions()
    {
        for (var i = 0; i < 20; i++)
        {
            var response = await Client.GetAsync($"/api/docs/projects/abp/no-such-{i}/languageList");
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        ServiceProvider.GetRequiredService<RecordingDistributedCache>().Writes.ShouldNotContain(x => x.Contains(nameof(LanguageConfig)));
        ServiceProvider.GetRequiredService<DocsGithubTestState>().LanguageDownloads.ShouldBe(0);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("latest")]
    public async Task Should_Resolve_Default_And_Latest_To_The_Configured_Branch(string version)
    {
        var suffix = version == null ? string.Empty : "?version=" + version;
        var code = await Client.GetStringAsync("/api/docs/projects/abp/defaultLanguage" + suffix);
        code.Trim('"').ShouldBe("en");

        var languages = await Client.GetFromJsonAsync<LanguageConfig>("/api/docs/projects/abp/dev/languageList");
        languages!.Languages.ShouldContain(x => x.Code == "tr");
        ServiceProvider.GetRequiredService<DocsGithubTestState>().LanguageDownloads.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Not_Duplicate_Configured_Branch_Prefixes()
    {
        var state = ServiceProvider.GetRequiredService<DocsGithubTestState>();
        state.VersionPrefix = "rel-";
        var projects = ServiceProvider.GetRequiredService<IProjectRepository>();
        var project = await projects.GetByShortNameAsync("abp");
        project.SetProperty("GithubVersionProviderSource", GithubVersionProviderSource.Branches);
        project.SetProperty("VersionBranchPrefix", "rel-");
        project.LatestVersionBranchName = "rel-2.0.0";
        await projects.UpdateAsync(project, true);

        var code = await Client.GetStringAsync("/api/docs/projects/abp/defaultLanguage?version=latest");
        code.Trim('"').ShouldBe("en");
        var languages = await Client.GetFromJsonAsync<LanguageConfig>("/api/docs/projects/abp/2.0.0/languageList");
        languages!.Languages.ShouldContain(x => x.Code == "tr");
        state.LastLanguageUrl.ShouldContain("/rel-2.0.0/");
        state.LanguageDownloads.ShouldBe(1);
    }
    [Fact]
    public async Task Should_Preserve_Cached_Languages_When_Version_Metadata_Is_Unavailable()
    {
        (await Client.GetFromJsonAsync<LanguageConfig>("/api/docs/projects/abp/2.0.0/languageList"))!
            .Languages.ShouldContain(x => x.Code == "tr");
        var project = await ServiceProvider.GetRequiredService<IProjectRepository>().GetByShortNameAsync("abp");
        await ServiceProvider.GetRequiredService<IDistributedCache<List<VersionInfo>>>()
            .RemoveAsync(CacheKeyGenerator.GenerateProjectVersionsCacheKey(project));
        ServiceProvider.GetRequiredService<DocsGithubTestState>().FailVersionDownloads = true;

        var cached = await Client.GetFromJsonAsync<LanguageConfig>("/api/docs/projects/abp/2.0.0/languageList");

        cached!.Languages.ShouldContain(x => x.Code == "tr");
        ServiceProvider.GetRequiredService<DocsGithubTestState>().LanguageDownloads.ShouldBe(1);
    }
}
