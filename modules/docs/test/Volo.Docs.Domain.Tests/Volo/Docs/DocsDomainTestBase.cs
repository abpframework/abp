using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Octokit;
using Volo.Docs.GitHub.Documents;

namespace Volo.Docs
{
    public abstract class DocsDomainTestBase : DocsTestBase<DocsDomainTestModule>
    {
        protected override void AfterAddApplication(IServiceCollection services)
        {
            var repositoryManager = Substitute.For<IGithubRepositoryManager>();
            repositoryManager.GetFileRawStringContentAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns("stringContent");
            repositoryManager.GetFileRawByteArrayContentAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(new byte[] { 0x01, 0x02, 0x03 });
            repositoryManager.GetReleasesAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(new List<Release>
                {
                    new Release("https://api.github.com/repos/abpframework/abp/releases/16293679",
                        "https://github.com/abpframework/abp/releases/tag/0.15.0",
                        "https://api.github.com/repos/abpframework/abp/releases/16293679/assets",
                        "https://uploads.github.com/repos/abpframework/abp/releases/16293679/assets{?name,label}",
                        16293679,
                        "0.15.0",
                        "master",
                        "0.15.0",
                        "0.15.0 already release",
                        false,
                        false,
                        DateTimeOffset.Parse("2019-03-22T18:43:58Z"),
                        DateTimeOffset.Parse("2019-03-22T19:44:25Z"),
                        null,
                        "https://api.github.com/repos/abpframework/abp/tarball/0.15.0",
                        "https://api.github.com/repos/abpframework/abp/zipball/0.15.0",
                        null)
                });
            services.AddSingleton(repositoryManager);
        }
    }
}
