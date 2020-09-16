using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Volo.Abp.Cli.Build
{
    public class BuildProjectListSorter_Tests : AbpCliTestBase
    {
        private IBuildProjectListSorter _buildProjectListSorter;

        public BuildProjectListSorter_Tests()
        {
            _buildProjectListSorter = GetRequiredService<IBuildProjectListSorter>();
        }

        [Fact]
        public void SortByDependencies_Test()
        {
            // A -> B, C
            // B -> D
            // D -> F
            // F -> C
            // C -> G
            // Final build order must be: G, 

            var repositoryName = "volo";
            var source = new List<DotNetProjectInfo>
            {
                new DotNetProjectInfo(repositoryName, "A", true)
                {
                    Dependencies = new List<DotNetProjectInfo>()
                    {
                        new DotNetProjectInfo(repositoryName, "B", true),
                        new DotNetProjectInfo(repositoryName, "C", true)
                    }
                },
                new DotNetProjectInfo(repositoryName, "B", true)
                {
                    Dependencies = new List<DotNetProjectInfo>()
                    {
                        new DotNetProjectInfo(repositoryName, "D", true)
                    }
                },
                new DotNetProjectInfo(repositoryName, "D", true)
                {
                    Dependencies = new List<DotNetProjectInfo>()
                    {
                        new DotNetProjectInfo(repositoryName, "F", true)
                    }
                },
                new DotNetProjectInfo(repositoryName, "F", true)
                {
                    Dependencies = new List<DotNetProjectInfo>()
                    {
                        new DotNetProjectInfo(repositoryName, "C", true)
                    }
                },
                new DotNetProjectInfo(repositoryName, "C", true)
                {
                    Dependencies = new List<DotNetProjectInfo>()
                    {
                        new DotNetProjectInfo(repositoryName, "G", true)
                    }
                },
                new DotNetProjectInfo(repositoryName, "G", true)
            };

            var sortedDependencies =
                _buildProjectListSorter.SortByDependencies(source, new DotNetProjectInfoEqualityComparer());
            sortedDependencies.Count.ShouldBe(6);
            sortedDependencies[0].CsProjPath.ShouldBe("G");
            sortedDependencies[1].CsProjPath.ShouldBe("C");
            sortedDependencies[2].CsProjPath.ShouldBe("F");
            sortedDependencies[3].CsProjPath.ShouldBe("D");
            sortedDependencies[4].CsProjPath.ShouldBe("B");
            sortedDependencies[5].CsProjPath.ShouldBe("A");
        }
    }
}
