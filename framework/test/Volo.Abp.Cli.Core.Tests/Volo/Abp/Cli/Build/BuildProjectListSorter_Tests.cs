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
                new DotNetProjectInfo(repositoryName, "A")
                {
                    Dependencies = new List<DotNetProjectInfo>()
                    {
                        new DotNetProjectInfo(repositoryName, "B"),
                        new DotNetProjectInfo(repositoryName, "C")
                    }
                },
                new DotNetProjectInfo(repositoryName, "B")
                {
                    Dependencies = new List<DotNetProjectInfo>()
                    {
                        new DotNetProjectInfo(repositoryName, "D")
                    }
                },
                new DotNetProjectInfo(repositoryName, "D")
                {
                    Dependencies = new List<DotNetProjectInfo>()
                    {
                        new DotNetProjectInfo(repositoryName, "F")
                    }
                },
                new DotNetProjectInfo(repositoryName, "F")
                {
                    Dependencies = new List<DotNetProjectInfo>()
                    {
                        new DotNetProjectInfo(repositoryName, "C")
                    }
                },
                new DotNetProjectInfo(repositoryName, "C")
                {
                    Dependencies = new List<DotNetProjectInfo>()
                    {
                        new DotNetProjectInfo(repositoryName, "G")
                    }
                },
                new DotNetProjectInfo(repositoryName, "G")
            };

            var sortedDependencies = _buildProjectListSorter.SortByDependencies(source, new DotNetProjectInfoEqualityComparer());
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
