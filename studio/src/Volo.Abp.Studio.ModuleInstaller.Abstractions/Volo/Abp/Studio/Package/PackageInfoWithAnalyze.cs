using JetBrains.Annotations;
using Volo.Abp.Studio.Analyzing.Models;

namespace Volo.Abp.Studio.Packages
{
    public class PackageInfoWithAnalyze
    {
        [NotNull]
        public string Path { get; }

        [NotNull]
        public string Name { get; }

        [CanBeNull]
        public string Role { get; }

        [NotNull]
        public PackageModel Analyze { get; }

        public PackageInfoWithAnalyze([NotNull] string path, [CanBeNull] string role, [NotNull] PackageModel analyze)
        {
            Path = Check.NotNullOrWhiteSpace(path, nameof(path));
            Name = PackageHelper.GetNameFromPath(path);
            Role = role;
            Analyze = Check.NotNull(analyze, nameof(analyze));
        }
    }
}
