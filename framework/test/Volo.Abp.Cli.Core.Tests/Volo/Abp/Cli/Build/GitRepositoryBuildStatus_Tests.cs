using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Volo.Abp.Cli.Build
{
    public class GitRepositoryBuildStatus_Tests : AbpCliTestBase
    {
        private IGitRepositoryHelper _gitRepositoryHelper;

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _gitRepositoryHelper = Substitute.For<IGitRepositoryHelper>();
            services.AddTransient(provider => _gitRepositoryHelper);
        }
        
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

        [Fact]
        public void Should_Update_Repository_CommitId_When_New_CommitId_Is_Not_Empty()
        {
            var existingBuildStatus = new GitRepositoryBuildStatus("volo", "dev");

            var newBuildStatus = new GitRepositoryBuildStatus(
                existingBuildStatus.RepositoryName,
                existingBuildStatus.BranchName
            )
            {
                CommitId = "42"
            };
            
            existingBuildStatus.MergeWith(newBuildStatus);
            
            existingBuildStatus.CommitId.ShouldBe("42");
        }
        
        [Fact]
        public void Should_Not_Update_Repository_CommitId_When_New_CommitId_Is_Empty()
        {
            var existingBuildStatus = new GitRepositoryBuildStatus("volo", "dev")
            {
                CommitId = "21"
            };

            var newBuildStatus = new GitRepositoryBuildStatus(
                existingBuildStatus.RepositoryName,
                existingBuildStatus.BranchName
            )
            {
                CommitId = ""
            };
            
            existingBuildStatus.MergeWith(newBuildStatus);
            
            existingBuildStatus.CommitId.ShouldBe("21");
        }

        [Fact]
        public void GetChild_Test()
        {
            var existingBuildStatus = new GitRepositoryBuildStatus("repo-1", "dev")
            {
                DependingRepositories = new List<GitRepositoryBuildStatus>()
                {
                    new GitRepositoryBuildStatus("repo-2", "dev")
                    {
                        DependingRepositories = new List<GitRepositoryBuildStatus>()
                        {
                            new GitRepositoryBuildStatus("repo-3", "dev")
                        }
                    },
                    new GitRepositoryBuildStatus("repo-4", "dev")
                }
            };
            
            existingBuildStatus.GetChild("repo-3").RepositoryName.ShouldBe("repo-3");
            existingBuildStatus.GetChild("repo-4").RepositoryName.ShouldBe("repo-4");
        }
    }
}
