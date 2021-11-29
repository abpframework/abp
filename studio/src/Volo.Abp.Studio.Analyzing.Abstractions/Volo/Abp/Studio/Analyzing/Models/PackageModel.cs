using JetBrains.Annotations;

namespace Volo.Abp.Studio.Analyzing.Models
{
    public class PackageModel
    {
        public string Name { get; }

        public string Hash { get; }
        
        public PackageContentList Contents { get; }

        public PackageModel([NotNull] string name, [NotNull] string hash)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Contents = new PackageContentList();
            Hash = hash;
        }
    }
}