using System.IO;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;

namespace Volo.Abp.Cli.Build
{
    public class FileSystemRepositoryBuildStatusStore : IRepositoryBuildStatusStore, ITransientDependency
    {
        public GitRepositoryBuildStatus Get(string buildNamePrefix, GitRepository repository)
        {
            if (!Directory.Exists(CliPaths.Build))
            {
                Directory.CreateDirectory(CliPaths.Build);
            }

            var buildStatusFile = Path.Combine(CliPaths.Build, repository.GetUniqueName(buildNamePrefix)) +
                                  ".json";

            if (!File.Exists(buildStatusFile))
            {
                return null;
            }

            var buildStatusText = File.ReadAllText(buildStatusFile);
            return JsonConvert.DeserializeObject<GitRepositoryBuildStatus>(buildStatusText);
        }

        public void Set(string buildNamePrefix, GitRepository repository, GitRepositoryBuildStatus status)
        {
            var existingRepositoryStatus = Get(buildNamePrefix, repository);

            var buildStatusFile = Path.Combine(
                CliPaths.Build,
                status.GetUniqueName(buildNamePrefix)
            ) + ".json";

            if (File.Exists(buildStatusFile))
            {
                FileHelper.DeleteIfExists(buildStatusFile);
            }

            if (existingRepositoryStatus != null)
            {
                existingRepositoryStatus.MergeWith(status);

                using (var file = File.CreateText(buildStatusFile))
                {
                    new JsonSerializer {Formatting = Formatting.Indented}.Serialize(file, existingRepositoryStatus);
                }
            }
            else
            {
                using (var file = File.CreateText(buildStatusFile))
                {
                    new JsonSerializer {Formatting = Formatting.Indented}.Serialize(file, status);
                }
            }
        }
    }
}
