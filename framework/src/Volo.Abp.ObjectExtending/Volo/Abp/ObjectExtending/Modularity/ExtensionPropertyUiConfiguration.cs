﻿using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending.Modularity
{
    public class ExtensionPropertyUiConfiguration
    {
        [NotNull]
        public ExtensionPropertyUiTableConfiguration OnTable { get; }

        [NotNull]
        public ExtensionPropertyUiFormConfiguration OnCreateForm { get; }

        [NotNull]
        public ExtensionPropertyUiFormConfiguration OnEditForm { get; }

        public ExtensionPropertyUiConfiguration()
        {
            OnTable = new ExtensionPropertyUiTableConfiguration();
            OnCreateForm = new ExtensionPropertyUiFormConfiguration();
            OnEditForm = new ExtensionPropertyUiFormConfiguration();
        }
    }
}