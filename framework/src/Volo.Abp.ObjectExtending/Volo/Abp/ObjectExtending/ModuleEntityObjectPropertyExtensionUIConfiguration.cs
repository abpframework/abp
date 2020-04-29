using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending
{
    public class ModuleEntityObjectPropertyExtensionUIConfiguration
    {
        [NotNull]
        public ModuleEntityObjectPropertyExtensionUITableConfiguration Table { get; }

        [NotNull]
        public ModuleEntityObjectPropertyExtensionUIFormConfiguration CreateForm { get; }

        [NotNull]
        public ModuleEntityObjectPropertyExtensionUIFormConfiguration EditForm { get; }

        public ModuleEntityObjectPropertyExtensionUIConfiguration()
        {
            Table = new ModuleEntityObjectPropertyExtensionUITableConfiguration();
            CreateForm = new ModuleEntityObjectPropertyExtensionUIFormConfiguration();
            EditForm = new ModuleEntityObjectPropertyExtensionUIFormConfiguration();
        }
    }
}