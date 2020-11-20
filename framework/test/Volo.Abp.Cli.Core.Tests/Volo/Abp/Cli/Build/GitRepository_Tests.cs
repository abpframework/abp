using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Volo.Abp.Cli.Build
{
    public class GitRepository_Tests : AbpCliTestBase
    {
        [Fact]
        public void GetUniqueName_Test()
        {
            var gitRepository = new GitRepository("repo-1", "dev", "")
            {
                DependingRepositories = new List<GitRepository>
                {
                    new GitRepository("repo-2", "dev", ""),
                    new GitRepository("repo-3", "dev", "")
                    {
                        DependingRepositories = new List<GitRepository>()
                        {
                            new GitRepository("repo-4", "dev", "")
                        }
                    }
                }
            };
            
            gitRepository.GetUniqueName("").ShouldBe("B25C935F97D7B3375530A96B392B7644");
            gitRepository.GetUniqueName("production").ShouldBe("production_B25C935F97D7B3375530A96B392B7644");
        }

        [Fact]
        public void FindRepositoryOf_Test()
        {
            var gitRepository = new GitRepository("repo-1", "dev", "/repo-1/dev/")
            {
                DependingRepositories = new List<GitRepository>
                {
                    new GitRepository("repo-2", "dev", "/repo-2/dev/"),
                    new GitRepository("repo-3", "dev", "/repo-3/dev/")
                    {
                        DependingRepositories = new List<GitRepository>()
                        {
                            new GitRepository("repo-4", "dev", "/repo-4/dev/")
                        }
                    }
                }
            };

            gitRepository.FindRepositoryOf("/repo-4/dev/A.csproj").ShouldBe("repo-4");
        }
    }
}
