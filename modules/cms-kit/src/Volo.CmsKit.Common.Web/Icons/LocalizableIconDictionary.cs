using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Volo.Abp;

namespace Volo.CmsKit.Web.Icons
{
    public class LocalizableIconDictionary : Dictionary<string, string>
    {
        [NotNull]
        public string Default
        {
            get => _default;
            set => _default = Check.NotNullOrWhiteSpace(value, nameof(value));
        }
        private string _default;

        public LocalizableIconDictionary([NotNull] string defaultIcon)
        {
            Default = defaultIcon;
        }

        public string GetLocalizedIconOrDefault(string cultureName = null)
        {
            cultureName ??= CultureInfo.CurrentUICulture.Name;
            
            var localizedIcon = this.GetOrDefault(cultureName);
            if (localizedIcon != null)
            {
                return localizedIcon;
            }

            return Default;
        }
    }
}
