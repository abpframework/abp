using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Octokit;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Docs.GitHub.Documents;
using Volo.Abp.Testing;
using Volo.Docs.GitHub.Documents.Version;
using Volo.Docs.Projects;

namespace Volo.Docs
{
    public abstract class DocsTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            var repositoryManager = Substitute.For<IGithubRepositoryManager>();
            repositoryManager.GetFileRawStringContentAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns("stringContent");
            repositoryManager.GetFileRawStringContentAsync(
                    Arg.Is<string>(x => x.Contains("docs-nav.json", StringComparison.InvariantCultureIgnoreCase)),
                    Arg.Any<string>(), Arg.Any<string>())
                .Returns("{\"items\":[{\"text\":\"Part-I.md\",\"path\":\"Part-I.md\"},{\"text\":\"Part-II\",\"path\":\"Part-II.md\"}]}");

            repositoryManager.GetFileRawByteArrayContentAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(new byte[] { 0x01, 0x02, 0x03 });
            repositoryManager.GetVersionsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), GithubVersionProviderSource.Releases)
                .Returns(new List<GithubVersion>
                {
                    new GithubVersion()
                    {
                        Name = "0.15.0"
                    }
                });
            repositoryManager.GetFileCommitsAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                    Arg.Any<string>(), Arg.Any<string>())
                .Returns(new List<GitHubCommit>
                {
                    new GitHubCommit("", "", "", "", "", null, null,
                        new Author("hikalkan ", 2, "", "https://avatars1.githubusercontent.com/u/1?v=4", "",
                            "https://github.com/hikalkan", "", "", "", "", "", "", "", "", "", "", false), "",
                        new Commit("", "", "", "", "", null, null, "", new Committer("", "", DateTimeOffset.Now),
                            null, null, new []{ new GitReference("", "", "", "", "", null, null) }, 1, null),
                        null, "", null, new []{ new GitReference("", "", "", "", "", null, null) }, null),

                    new GitHubCommit("", "", "", "", "", null, null,
                        new Author("ebicoglu ", 2, "", "https://avatars1.githubusercontent.com/u/2?v=4", "",
                            "https://github.com/ebicoglu", "", "", "", "", "", "", "", "", "", "", false), "",
                        new Commit("", "", "", "", "", null, null, "", new Committer("", "", DateTimeOffset.Now),
                            null, null, new []{ new GitReference("", "", "", "", "", null, null) }, 1, null),
                        null, "", null, new []{ new GitReference("", "", "", "", "", null, null) }, null)
                });

            services.AddSingleton(repositoryManager);
        }
    }
}
