using System.IO;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;

namespace Volo.Abp.Cli.Build
{
    public class FileSystemRepositoryBuildStatusStore : IRepositoryBuildStatusStore, ITransientDependency
    {
        // TODO: change this ?
        private string BaseBuildStatusStorePath = @"C:\Users\ismai\.abp\build";

        public GitRepositoryBuildStatus Get(string buildNamePrefix, GitRepository repository)
        {
            if (!Directory.Exists(BaseBuildStatusStorePath))
            {
                Directory.CreateDirectory(BaseBuildStatusStorePath);
            }

            var buildStatusFile = Path.Combine(BaseBuildStatusStorePath, repository.GetUniqueName(buildNamePrefix)) +
                                  ".json";

            if (!File.Exists(buildStatusFile))
            {
                // TODO: this is wrong
                return new GitRepositoryBuildStatus(repository.Name, repository.BranchName);
            }

            var buildStatusText = File.ReadAllText(buildStatusFile);
            return JsonConvert.DeserializeObject<GitRepositoryBuildStatus>(buildStatusText);
        }

        public void Set(string buildNamePrefix, GitRepository repository, GitRepositoryBuildStatus status)
        {
            var existingRepositoryStatus = Get(buildNamePrefix, repository);

            var buildStatusFile = Path.Combine(BaseBuildStatusStorePath, status.GetUniqueName(buildNamePrefix)) + ".json";
            if (File.Exists(buildStatusFile))
            {
                FileHelper.DeleteIfExists(buildStatusFile);
            }

            existingRepositoryStatus.MergeWith(status);

            using (var file = File.CreateText(buildStatusFile))
            {
                new JsonSerializer {Formatting = Formatting.Indented}.Serialize(file, existingRepositoryStatus);
            }
        }

        // private void UpdateBuildStatus(
        //     GitRepositoryBuildStatus existingBuildStatus,
        //     GitRepositoryBuildStatus newBuildStatus)
        // {
        //
        //     foreach (var succeedProject in newBuildStatus.SucceedProjects)
        //     {
        //         existingBuildStatus.UpdateProjectStatus(succeedProject);
        //
        //         foreach (var dependingRepositoryBuildStatus in newBuildStatus.DependingRepositories)
        //         {
        //             UpdateBuildStatus(existingBuildStatus, dependingRepositoryBuildStatus);
        //         }
        //     }
        // }
    }
}
