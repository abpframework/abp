using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

namespace Volo.Abp.Cli.Build
{
    public class GitRepositoryBuildStatus_Tests : AbpCliTestBase
    {
        [Fact]
        public void Add_New_Build_Status_Test()
        {
            var existingBuildStatus = new GitRepositoryBuildStatus("volo", "dev")
            {
                SucceedProjects = new List<DotNetProjectBuildStatus>
                {
                    new DotNetProjectBuildStatus
                    {
                        CsProjPath = "project1.csproj",
                        CommitId = "1"
                    }
                }
            };

            var newBuildStatus = new GitRepositoryBuildStatus(
                existingBuildStatus.RepositoryName,
                existingBuildStatus.BranchName
            )
            {
                SucceedProjects = new List<DotNetProjectBuildStatus>
                {
                    new DotNetProjectBuildStatus
                    {
                        CsProjPath = "project2.csproj",
                        CommitId = "2"
                    }
                }
            };

            existingBuildStatus.MergeWith(newBuildStatus);
            
            existingBuildStatus.SucceedProjects.Count.ShouldBe(2);
        }
        
        [Fact]
        public void Update_Existing_Build_Status_Test()
        {
            var existingBuildStatus = new GitRepositoryBuildStatus("volo", "dev")
            {
                SucceedProjects = new List<DotNetProjectBuildStatus>
                {
                    new DotNetProjectBuildStatus
                    {
                        CsProjPath = "project1.csproj",
                        CommitId = "1"
                    }
                }
            };

            var newBuildStatus = new GitRepositoryBuildStatus(
                existingBuildStatus.RepositoryName,
                existingBuildStatus.BranchName
            )
            {
                SucceedProjects = new List<DotNetProjectBuildStatus>
                {
                    new DotNetProjectBuildStatus
                    {
                        CsProjPath = "project1.csproj",
                        CommitId = "2"
                    },
                    new DotNetProjectBuildStatus
                    {
                        CsProjPath = "project2.csproj",
                        CommitId = "2"
                    }
                }
            };

            existingBuildStatus.MergeWith(newBuildStatus);
            existingBuildStatus.SucceedProjects.Count.ShouldBe(2);
            existingBuildStatus.GetSelfOrChild("volo").SucceedProjects.First(p => p.CsProjPath == "project1.csproj").CommitId.ShouldBe("2");
            existingBuildStatus.GetSelfOrChild("volo").SucceedProjects.First(p => p.CsProjPath == "project2.csproj").CommitId.ShouldBe("2");
        }
        
        [Fact]
        public void Add_New_Build_Status_For_Child_Repository_Test()
        {
            var existingBuildStatus = new GitRepositoryBuildStatus("volo", "dev")
            {
                DependingRepositories = new List<GitRepositoryBuildStatus>()
                {
                    new GitRepositoryBuildStatus("abp","dev")
                    {
                        SucceedProjects = new List<DotNetProjectBuildStatus>
                        {
                            new DotNetProjectBuildStatus
                            {
                                CsProjPath = "project1.csproj",
                                CommitId = "1"
                            }
                        }
                    }
                }
            };

            var newBuildStatus = new GitRepositoryBuildStatus(
                existingBuildStatus.RepositoryName,
                existingBuildStatus.BranchName
            )
            {
                DependingRepositories = new List<GitRepositoryBuildStatus>()
                {
                    new GitRepositoryBuildStatus("abp","dev")
                    {
                        SucceedProjects = new List<DotNetProjectBuildStatus>
                        {
                            new DotNetProjectBuildStatus
                            {
                                CsProjPath = "project2.csproj",
                                CommitId = "2"
                            }
                        }
                    }
                }
            };

            existingBuildStatus.MergeWith(newBuildStatus);
            existingBuildStatus.GetChild("abp").SucceedProjects.Count.ShouldBe(2);
        }
    }
}
