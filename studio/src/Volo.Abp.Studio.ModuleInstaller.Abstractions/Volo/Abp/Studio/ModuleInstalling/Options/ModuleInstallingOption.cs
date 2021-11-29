using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Studio.ModuleInstalling.Options
{
    public class ModuleInstallingOption
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ModuleInstallingOptionType Type { get; set; }

        public Dictionary<string, string> Values { get; set; }

        public ModuleInstallingOption(
            [NotNull] string name,
            [NotNull] string description,
            [NotNull] ModuleInstallingOptionType type,
            [CanBeNull] Dictionary<string, string> values = null)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            Type = type;
            Values = values ?? new Dictionary<string, string>();
        }
    }
}