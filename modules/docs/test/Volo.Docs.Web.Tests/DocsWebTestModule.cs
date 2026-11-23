using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Octokit;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Modularity;
using Volo.Abp.Data;
using Volo.Docs.Common;
using Volo.Docs.Common.Documents;
using Volo.Docs.EntityFrameworkCore;
using Volo.Docs.FileSystem.Documents;
using Volo.Docs.GitHub.Documents;
using Volo.Docs.GitHub.Documents.Version;
using Volo.Docs.Projects;
using Project = Volo.Docs.Projects.Project;

namespace Volo.Docs;

[DependsOn(typeof(AbpAspNetCoreTestBaseModule), typeof(DocsWebModule), typeof(DocsApplicationModule),
    typeof(DocsCommonHttpApiModule), typeof(DocsEntityFrameworkCoreTestModule))]
public class DocsWebTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(builder => builder.PartManager.ApplicationParts.Add(new CompiledRazorAssemblyPart(typeof(DocsWebModule).Assembly)));
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<RecordingDistributedCache>();
        context.Services.AddSingleton<Microsoft.Extensions.Caching.Distributed.IDistributedCache>(provider => provider.GetRequiredService<RecordingDistributedCache>());
        context.Services.AddDataProtection().UseEphemeralDataProtectionProvider();
        context.Services.AddSingleton(Substitute.For<IDocumentPdfAppService>());
        context.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("en");
            options.SupportedCultures = new[] { new CultureInfo("en"), new CultureInfo("tr") };
            options.SupportedUICultures = options.SupportedCultures;
        });
        Configure<AbpBundlingOptions>(options => options.Mode = BundlingMode.None);
        Configure<DocsUiOptions>(options => options.ShowProjectsCombobox = false);
        var theme = Substitute.For<ITheme>();
        theme.GetEmptyLayout().Returns("/Pages/TestLayout.cshtml");
        var themes = Substitute.For<IThemeManager>();
        themes.CurrentTheme.Returns(theme);
        context.Services.AddSingleton(themes);

        var state = new DocsGithubTestState();
        context.Services.AddSingleton(state);
        var github = Substitute.For<IGithubRepositoryManager>();
        github.GetVersionsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<GithubVersionProviderSource>())
            .Returns(call =>
            {
                if (state.FailVersionDownloads)
                {
                    throw new HttpRequestException("Temporary test metadata failure.");
                }

                return new List<GithubVersion> { new() { Name = state.VersionPrefix + "2.0.0" }, new() { Name = state.VersionPrefix + "1.0.0" } };
            });
        github.GetFileRawStringContentAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(call =>
        {
            var url = call.ArgAt<string>(0);
            if (url.Contains("no-such-") || url.Contains("//docs/"))
            {
                throw new HttpRequestException("Unknown test version.");
            }

            if (url.EndsWith("docs-langs.json"))
            {
                state.LanguageDownloads++;
                state.LastLanguageUrl = url;
                if (state.FailLanguageDownloads)
                {
                    throw new HttpRequestException("Temporary test transport failure.");
                }

                return url.Contains("/1.0.0/")
                    ? "{\"languages\":[{\"displayName\":\"English\",\"code\":\"en\",\"isDefault\":true}]}"
                    : File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "docs-langs.json"));
            }

            if (url.EndsWith("docs-nav.json"))
            {
                return File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "en", "docs-nav.json"));
            }

            if (url.EndsWith("docs-params.json"))
            {
                return "{\"parameters\":[]}";
            }

            return "# Getting started\n\nVersion {{ Document_Version }}.";
        });
        github.GetFileCommitsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(new List<GitHubCommit>());
        context.Services.AddSingleton(github);
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        using (var scope = context.ServiceProvider.CreateScope())
        {
            var projects = scope.ServiceProvider.GetRequiredService<IProjectRepository>();
            var project = new Project(Guid.Parse("60e52348-2719-4d39-8af9-ced46d6c815a"), "Filesystem docs", "product", FileSystemDocumentSource.Type,
                "md", "Installation.md", "docs-nav.json", "docs-params.json") { LatestVersionBranchName = "1.0.0" };
            project.SetProperty("Path", Path.Combine(AppContext.BaseDirectory, "Fixtures"));
            await projects.InsertAsync(project, true);
            var githubProject = await projects.GetByShortNameAsync("abp");
            githubProject.LatestVersionBranchName = "dev";
            await projects.UpdateAsync(githubProject, true);
        }

        var app = context.GetApplicationBuilder();
        app.UseRequestLocalization();
        app.UseRouting();
        app.UseAuthorization();
        app.UseConfiguredEndpoints();
    }
}

public class DocsGithubTestState
{
    public bool FailLanguageDownloads { get; set; }
    public bool FailVersionDownloads { get; set; }
    public int LanguageDownloads { get; set; }
    public string LastLanguageUrl { get; set; }
    public string VersionPrefix { get; set; } = string.Empty;
}
