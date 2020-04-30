using System;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending
{
    [Serializable]
    public class LocalizableStringDto
    {
        public string Name { get; set; }

        [CanBeNull]
        public string Resource { get; set; }
    }
}