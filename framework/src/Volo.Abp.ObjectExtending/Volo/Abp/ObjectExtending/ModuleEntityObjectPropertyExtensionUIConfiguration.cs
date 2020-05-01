using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending
{
    public class ModuleEntityObjectPropertyExtensionUIConfiguration
    {
        [NotNull]
        public ModuleEntityObjectPropertyExtensionUITableConfiguration OnTable { get; }

        [NotNull]
        public ModuleEntityObjectPropertyExtensionUIFormConfiguration OnCreateForm { get; }

        [NotNull]
        public ModuleEntityObjectPropertyExtensionUIFormConfiguration OnEditForm { get; }

        public ModuleEntityObjectPropertyExtensionUIConfiguration()
        {
            OnTable = new ModuleEntityObjectPropertyExtensionUITableConfiguration();
            OnCreateForm = new ModuleEntityObjectPropertyExtensionUIFormConfiguration();
            OnEditForm = new ModuleEntityObjectPropertyExtensionUIFormConfiguration();
        }
    }
}