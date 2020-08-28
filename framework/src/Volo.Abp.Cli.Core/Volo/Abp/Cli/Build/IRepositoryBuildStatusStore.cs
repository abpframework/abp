using System.IO;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;

namespace Volo.Abp.Cli.Build
{
    public interface IRepositoryBuildStatusStore
    {
        GitRepositoryBuildStatus Get(GitRepository repository);

        void Set(GitRepositoryBuildStatus status);
    }

    public class FileSystemRepositoryBuildStatusStore : IRepositoryBuildStatusStore, ITransientDependency
    {
        // TODO: change this ?
        private string BaseBuildStatusStorePath = @"C:\Users\ismai\.abp\build";

        public GitRepositoryBuildStatus Get(GitRepository repository)
        {
            if (!Directory.Exists(BaseBuildStatusStorePath))
            {
                Directory.CreateDirectory(BaseBuildStatusStorePath);
            }

            var buildStatusFile = Path.Combine(BaseBuildStatusStorePath, repository.GetUniqueName()) + ".json";

            if (!File.Exists(buildStatusFile))
            {
                // TODO: this is wrong
                return new GitRepositoryBuildStatus(repository.Name, repository.BranchName);
            }

            var buildStatusText = File.ReadAllText(buildStatusFile);
            return JsonConvert.DeserializeObject<GitRepositoryBuildStatus>(buildStatusText);
        }

        public void Set(GitRepositoryBuildStatus status)
        {
            if (!Directory.Exists(BaseBuildStatusStorePath))
            {
                Directory.CreateDirectory(BaseBuildStatusStorePath);
            }

            var buildStatusFile = Path.Combine(BaseBuildStatusStorePath, status.GetUniqueName()) + ".json";
            if (File.Exists(buildStatusFile))
            {
                FileHelper.DeleteIfExists(buildStatusFile);
            }

            using (var file = File.CreateText(buildStatusFile))
            {
                new JsonSerializer {Formatting = Formatting.Indented}.Serialize(file, status);
            }
        }
    }
}
