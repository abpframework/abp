using System;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    [Serializable]
    public class LocalizableStringDto
    {
        [NotNull]
        public string Name { get; private set; }

        [CanBeNull]
        public string Resource { get; set; }

        public LocalizableStringDto([NotNull] string name, string resource = null)
        {
            Name = Check.NotNullOrEmpty(name, nameof(name));
            Resource = resource;
        }
    }
}