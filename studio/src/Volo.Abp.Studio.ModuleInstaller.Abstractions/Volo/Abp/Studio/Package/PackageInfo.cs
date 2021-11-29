using JetBrains.Annotations;
using Volo.Abp.Studio.Analyzing;

namespace Volo.Abp.Studio.Packages
{
    public class PackageInfo
    {
        [NotNull]
        public string Path { get; }
        
        [NotNull]
        public string Name { get; }
        
        [CanBeNull]
        public string Role { get; }
        
        [CanBeNull]
        public AnalyzingOptions AnalyzingOptions { get; set; }

        public PackageInfo([NotNull] string path, [CanBeNull] string role)
        {
            Path = Check.NotNullOrWhiteSpace(path, nameof(path));
            Name = PackageHelper.GetNameFromPath(path);
            Role = role;
        }
    }
}